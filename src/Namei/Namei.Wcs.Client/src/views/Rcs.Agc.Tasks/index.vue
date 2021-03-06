<template>
  <AsyncLoader
    :handler="getTasks"
    class="has-background-white"
    style="padding: 1.25rem; overflow: auto"
  >
    <div class="is-flex">
      <SearchField @search="handleSearch" />

      <div class="is-flex-auto"></div>

      <router-link
        class="button is-info"
        :to="{ name: 'NameiWcsRcsAgcTasksCreate' }"
      >
        创建任务
      </router-link>
    </div>

    <table class="table is-fullwidth is-bordered is-centered is-nowrap">
      <thead>
        <th>#</th>
        <th>任务类型</th>
        <th>状态</th>
        <th>外部单号</th>
        <th>起点</th>
        <th>终点</th>
        <th>托盘码</th>
        <th>AGC 编号</th>
        <th>RCS 任务单号</th>
        <th>日期</th>
        <th></th>
      </thead>
      <tbody>
        <DataMapIterator
          :dataMap="data"
          v-slot="{ entity }"
          tag="tr"
        >
          <td>{{entity.id}}</td>
          <td>{{entity.type.name}}</td>
          <TheStatus :value="entity.status" />
          <td>{{entity.taskId}}</td>
          <td>{{entity.position}}</td>
          <td>{{entity.destination}}</td>
          <td>{{entity.podCode}}</td>
          <td>{{entity.agcCode}}</td>
          <td>{{entity.rcsTaskCode}}</td>
          <td v-text="entity.createdAt" tag="td" />
          <td>
            <a @click="handleClose(entity.id)">
              关闭
            </a>

            <a @click="handleFinish(entity.id)">
              完成
            </a>
          </td>
        </DataMapIterator>
      </tbody>
    </table>

    <div style="height: 1.25rem"></div>

    <Pagination
      v-bind="data"
      @change="changePage"
    />

    <router-view @refresh="getTasks" />
  </AsyncLoader>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { useConfirm } from "@midos/vue-ui";
import { usePagination } from "../../hooks/use-pagination";
import { useQuery } from "../../hooks/use-query";
import { useWcsHttp } from "../../services/wcs-http";
import SearchField from "../../components/SearchField.vue";
import TheStatus from "./TheStatus.vue";

export default defineComponent({
  name: "PickTicketTasks",

  components: {
    TheStatus,
    SearchField
  },

  setup() {
    const http = useWcsHttp();
    const param = useQuery(50);
    const data = usePagination();
    const confirm = useConfirm();

    async function getTasks() {
      data.value = await http.paginate("/agc-tasks/search", param);
    }

    function changePage(page: number) {
      param.page = page;

      return getTasks();
    }

    function handleSearch(query: string) {
      param.query = query;

      return getTasks();
    }

    function handleClose(id: number) {
      confirm.open({
        title: "提示",
        content: "确认后任务将被关闭",
        handler: async () => {
          await http.post("/agc-tasks/close", { id });
          await getTasks();
        }
      });
    }

    function handleFinish(id: number) {
      confirm.open({
        title: "提示",
        content: "手动执行任务完成",
        handler: async() => {
          await http.post("/agc-tasks/finish", { id, agcCode: "0" });
        }
      });
    }

    return {
      data,
      getTasks,
      changePage,
      handleSearch,
      handleClose,
      handleFinish
    };
  }
});
</script>
