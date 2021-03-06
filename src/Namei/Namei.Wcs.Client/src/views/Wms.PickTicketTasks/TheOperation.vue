<template>
  <span style="cursor: default">
    <div
      v-if="isShow"
      class="modal is-active"
      style="text-align: left"
    >
      <div class="modal-background"></div>
      <div class="modal-card" style="width: 600px">
        <div class="modal-card-head">
          <p class="modal-card-title">
            任务下发
          </p>
          <button
            class="delete"
            @click="isShow = false"
          />
        </div>
        <div class="modal-card-body">
          <div class="field">
            <label class="label">
              车间
            </label>
            <div class="control">
              <Input :value="entity.fromName" readonly />
            </div>
          </div>

          <div class="field">
            <label class="label">
              捡货单号
            </label>
            <div class="control">
              <Input :value="entity.orderNumber" readonly />
            </div>
          </div>

          <div class="field">
            <label class="label">
              货名
            </label>
            <div class="control">
              <Input :value="entity.itemName" readonly />
            </div>
          </div>

          <div class="field">
            <label class="label">
              货码
            </label>
            <div class="control">
              <Input :value="entity.itemCode" readonly />
            </div>
          </div>

          <div class="field">
            <label class="label">
              托盘码
            </label>
            <div class="control">
              <Input v-model:value="params.palletCode" />
            </div>
          </div>

          <div class="field">
            <label class="label">
              起点
            </label>
            <div class="control">
              <Input v-model:value="params.position" />
            </div>
          </div>

          <div class="field">
            <label class="label">
              终点
            </label>
            <div class="control">
              <Input v-model:value="params.destination" />
            </div>
          </div>
        </div>
        <div class="modal-card-foot is-flex">
          <AsyncButton
            class="button is-success"
            :handler="handleSubmit"
          >
            下发
          </AsyncButton>

          <AsyncButton
            class="button is-danger"
            :handler="handleClose"
          >
            关闭
          </AsyncButton>
        </div>
      </div>
    </div>
    <a
      v-if="entity.status === null"
      @click="isShow = true"
    >
      下发
    </a>
    <a
      @click="handleClose"
      v-else-if="entity.status === 'started'"
    >
      关闭
    </a>
    <a
      v-else
      @click="handleReset"
    >
      重置
    </a>
  </span>
</template>

<script lang="ts">
import { defineComponent, PropType, ref } from "vue";
import { useConfirm } from "@midos/vue-ui";
import { useRcsExtHttp } from "../../services/rcs-ext-http";
import { WmsPickTicketTask } from "./entities/pick-ticket-task";

export default defineComponent({
  name: "TheOperation",

  props: {
    entity: {
      type: Object as PropType<WmsPickTicketTask>,
      required: true
    },
    restQuantity: {
      type: Number,
      required: true
    }
  },

  setup(props, { emit }) {
    const confirm = useConfirm();
    const http = useRcsExtHttp();
    const isShow = ref(false);
    const params = ref({
      taskId: props.entity.id,
      position: props.entity.locationCode,
      // eslint-disable-next-line no-template-curly-in-string
      destination: props.restQuantity ? "204${04}" : "294${04}",
      palletCode: props.entity.palletCode,
    });

    async function handleSubmit() {
      await http.post("/wms/pick-ticket-tasks/start", params.value);
      isShow.value = false;
      emit("refresh");
    }

    function handleClose() {
      confirm.open({
        title: "关闭任务",
        content: "确定后任务将被关闭",
        handler: async () => {
          await http.post(
            "/wms/pick-ticket-tasks/close",
            { id: props.entity.id }
          );
          isShow.value = false;
          await emit("refresh");
        }
      });
    }

    function handleReset() {
      confirm.open({
        title: "重置任务",
        content: "确认后任务将被重置",
        handler: async () => {
          await http.post(
            "/wms/pick-ticket-tasks/reset",
            { id: props.entity.id }
          );
          await emit("refresh");
        }
      });
    }

    return {
      isShow,
      params,
      handleSubmit,
      handleClose,
      handleReset
    };
  },
});
</script>
