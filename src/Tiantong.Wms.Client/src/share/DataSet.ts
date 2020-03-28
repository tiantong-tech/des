import { Vue, Component, Prop } from 'vue-property-decorator'
import Axios from 'axios'

interface IString {
  toString() : string
}

@Component({})
export default class DataSet<TKey extends IString = number> extends Vue {
  //
  protected api!: string

  protected page: number = 1

  protected pageSize: number = 20

  protected search?: string = undefined

  get params (): object {
    return {}
  }

  //

  isPending: boolean = false

  entities: Entities<TKey> = new Entities<TKey>()

  // computed

  get entityList () {
    const { keys, data } = this.entities

    return keys.map(key => data[key.toString()])
  }

  // methods

  getEntity (key: never)  {
    return this.entities.data[key]
  }

  async getEntities () {
    if (this.isPending) return

    const params = Object.assign({
      page: this.page,
      page_size: this.pageSize,
      search: this.search,
    }, this.params)

    try {
      const response = await Axios.post(this.api, params)

      this.entities = response.data
    } finally {
      this.isPending = false
    }
  }

  async handleSearch (value: string) {
    this.search = value !== '' ? value : undefined
    await this.getEntities()
  }

  async handlePageChange (page: number) {
    this.page = page
    await this.getEntities()
  }

  async handlePageSizeChange (pageSize: number) {
    this.page = 1
    this.pageSize = pageSize
    await this.getEntities()
  }

  async handleRefresh () {
    await this.handlePageChange(1)
  }
}

class Entities<TKey extends IString> {
  meta = {
    page: 1,
    pageSize: 1,
    total: 1,
  }

  keys: TKey[] = new Array<TKey>()

  data: { [key: string]: object } = {}

  relationships?: object = {}
}
