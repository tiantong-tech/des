<template>
  <AsyncLoader :handler="getEntities">
    <div class="is-flex">
      <SearchField
        :isPending="isPending"
        @search="handleSearch"
      />
      <div class="is-flex-auto"></div>
      <a
        class="button is-info"
        @click="$router.push(`/warehouses/${warehouseId}/items/create`)"
      >
        添加
      </a>
    </div>

    <Table v-show="!isPending">
      <thead>
        <th>货码</th>
        <th>货名</th>
        <th style="width: 1px">数量</th>
        <th style="width: 1px">规格</th>
        <th>备注</th>
        <th>启用中</th>
        <th style="width: 100px">操作</th>
      </thead>
      <tbody>
        <tr v-for="item in entityList" :key="item.id">
          <td>{{item.number}}</td>
          <td>{{item.name}}</td>
          <!-- <td>{{item.stocks.reduce((s, stock) => s += stock.quantity, 0)}}</td> -->
          <td>{{item.specification}}</td>
          <td>{{item.comment}}</td>
          <td>
            <YesOrNoCell :value="item.is_enabled"></YesOrNoCell>
          </td>
          <td>
            <router-link :to="`/warehouses/${warehouseId}/items/${item.id}/update`">
              <span class="icon is-info">
                <i class="iconfont icon-edit"></i>
              </span>
              <span>编辑</span>
            </router-link>
          </td>
        </tr>
      </tbody>
    </Table>
    <div style="height: 1rem"></div>
    <Pagination v-show="!isPending" v-bind="entities.meta" @change="handlePageChange"></Pagination>

    <router-view
      :warehouseId="warehouseId"
      @refresh="handleRefresh"
    ></router-view>
  </AsyncLoader>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import DataSet from '@/share/DataSet'
import Table from '@/components/Table.vue'
import SearchField from '@/components/SearchField.vue'
import YesOrNoCell from '@/components/YesOrNoCell.vue'
import Pagination from '@/components/Pagination.vue'
import AsyncLoader from '@/components/AsyncLoader.vue'
import DateWrapper from '@/components/wrappers/DateWrapper.vue'

@Component({
  name: 'Stocks',
  components: {
    Table,
    Pagination,
    SearchField,
    YesOrNoCell,
    AsyncLoader,
    DateWrapper
  }
})
export default class extends DataSet {
  @Prop({ required: true })
  warehouseId!: number

  get params () {
    return {
      warehouse_id: this.warehouseId
    }
  }

  public constructor() {
    super()
    this.api = '/goods/search'
  }
}
</script>
