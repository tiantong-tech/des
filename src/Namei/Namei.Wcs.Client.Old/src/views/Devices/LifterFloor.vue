<template>
  <div
    class="is-flex"
    style="height: 110px"
  >
    <LifterPlatform
      :code="floorState.palletCodeB"
      text="B 段"
    />

    <LifterPlatform
      :code="floorState.palletCodeA"
      text="A 段"
    />

    <div
      class="is-flex is-flex-column"
      style="width: 100px"
    >
      <div class="is-flex-auto"></div>

      <div
        class="is-flex is-flex-column"
        style="width: 100px; margin-left: 0.5rem"
      >
        <div class="is-flex-auto"></div>

        <div class="is-flex is-centered">
          <a
            class="tag"
            v-class:is-success="isExported"
            style="margin-right: 0.125rem"
            @click="handleTaken"
          >
            取货
          </a>
          <a
            class="tag"
            v-class:is-success="isImportAllowed"
            @click="handleImported"
          >
            放货
          </a>
        </div>

        <div class="is-flex is-centered is-vcentered" style="height: 30px">
          <span
            v-if="!isAgcRequesting"
            class="tag is-light"
          >
            AGC 请求通过
          </span>
          <a
            v-else
            @click="handleOpenDoor"
            class="tag is-info"
          >
            AGC 请求通过
          </a>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import Vue from 'vue'
import domain from '@/providers/contexts/domain'
import LifterPlatform from './LifterPlatform.vue'

export default Vue.extend({
  name: 'LifterFloor',

  components: {
    LifterPlatform
  },

  props: {
    lifterId: {
      type: String,
      required: true
    },
    floor: {
      type: Number,
      required: true
    },
    floorState: {
      type: Object,
      required: true
    },
    door: {
      type: Object,
      required: true
    }
  },

  computed: { 
    isAgcRequesting () {
      return !!this.door.requestingTasks.length
    },

    isImportAllowed () {
      return this.floorState.isImportAllowed
    },

    isExported () {
      return this.floorState.isExported
    }
  },

  methods: {
    handleOpenDoor () {
      this.$confirm({
        title: '确认',
        content: '确认后将直接允许 AGC 通过',
        handler: () => domain.post('/doors/control', {
          command: 'open',
          door_id: this.door.id,
        }),
      })
    },

    handleImported () {
      this.$confirm({
        title: '放货',
        content: '发送放货完成信号',
        handler: () => domain.post('/lifters/imported', {
          lifterId: this.lifterId,
          floor: this.floor.toString()
        })
      })
    },

    handleTaken () {
      this.$confirm({
        title: '取货',
        content: '发送取货完成信号',
        handler: () => domain.post('/lifters/taken', {
          lifterId: this.lifterId,
          floor: this.floor.toString()
        })
      })
    }
  }
})
</script>
