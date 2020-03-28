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
        @click="$router.push(`/warehouses/${warehouseId}/suppliers/create`)"
      >
        添加
      </a>
    </div>

    <Table v-show="!isPending">
      <thead>
        <th>供应商名</th>
        <th>备注</th>
        <th>启用中</th>
        <th style="width: 100px">操作</th>
      </thead>
      <tbody>
        <tr v-for="supplier in entityList" :key="supplier.id">
          <td>{{supplier.name}}</td>
          <td>{{supplier.comment}}</td>
          <td>
            <YesOrNoCell :value="supplier.is_enabled"></YesOrNoCell>
          </td>
          <td>
            <router-link :to="`/warehouses/${warehouseId}/suppliers/${supplier.id}/update`">
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

@Component({
  name: 'Suppliers',
  components: {
    Table,
    Pagination,
    SearchField,
    YesOrNoCell,
    AsyncLoader,
  }
})
export default class extends DataSet {
  @Prop({ required: true })
  warehouseId!: number

  public constructor() {
    super()
    this.api = '/suppliers/search'
  }

  get params () {
    return { warehouse_id: this.warehouseId }
  }
}
</script>
