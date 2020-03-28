<template>
  <div class="modal is-active">
    <div class="modal-background"></div>
    <div class="modal-card">
      <header class="modal-card-head">
        <p class="modal-card-title">货品添加</p>
      </header>
      <section class="modal-card-body">
        <div class="field">
          <div class="label">
            <label>货码</label>
          </div>
          <div class="control">
            <input v-model.lazy="params.number" type="text" class="input">
          </div>
        </div>

        <div class="field">
          <div class="label">
            <label>货名</label>
          </div>
          <div class="control">
            <input v-model.lazy="params.name" type="text" class="input">
          </div>
        </div>

        <div class="field">
          <div class="label">
            <label>规格</label>
          </div>
          <div class="control">
            <input v-model.lazy="params.specification" type="text" class="input">
          </div>
        </div>

        <div class="field">
          <div class="label">
            <label>备注</label>
          </div>
          <div class="control">
            <Textarea v-model="params.comment"></Textarea>
          </div>
        </div>
      </section>
      <footer class="modal-card-foot">
        <AsyncButton
          :disabled="!isChanged"
          :handler="handleSubmit"
          class="button is-success"
        >
          提交
        </AsyncButton>
        <button
          class="button"
          @click="handleCancel"
        >
          取消
        </button>
      </footer>
    </div>
  </div>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import Textarea from '@/components/Textarea.vue'
import AsyncButton from '@/components/AsyncButton.vue'
import axios from '@/providers/axios'
import DatePicker from '@/components/DatePicker/index.vue'

@Component({
  name: 'ProjectCreate',
  components: {
    Textarea,
    AsyncButton,
    DatePicker
  }
})
export default class extends Vue {
  @Prop({ required: true})
  warehouseId!: number

  params = {
    name: '',
    number: null,
    comment: '',
    specification: '个',
    warehouse_id: this.warehouseId
  }

  isPending: boolean = false

  get isChanged () {
    return this.params.name !== ''
  }

  handleCancel () {
    this.$router.go(-1)
  }

  async handleSubmit () {
    try {
      await axios.post('/items/create', this.params)
      this.handleCancel()
      this.$emit('refresh')
    } finally {}
  }
}
</script>
