<template>
  <div
    class="is-flex is-flex-column"
    style="overflow: auto;"
  >
    <div
      class="tabs"
      style="margin-bottom: 0; font-size: 1.125rem; flex-shrink: 0"
    >
      <ul>
        <router-link
          to="/doors/states"
          tag="li"
        >
          <a>设备状态</a>
        </router-link>
        <router-link
          to="/doors/commands"
          tag="li"
        >
          <a>控制指令</a>
        </router-link>
      </ul>
    </div>

    <router-view class="is-flex-auto" />
  </div>
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator'

@Component({
  name: 'Doors'
})
export default class extends Vue {

}
</script>
