import type { PagingFindQuery } from '/@/api/model/baseModel'

export interface BlockingQuery extends PagingFindQuery {
  templateName?: string
}

export interface BlockingVM {
  id: string
  templateName: string
  createOn: Date
  updateOn: Date
  blacklists: string[]
}

export interface CreateBlockingCommand {
  templateName: string
  blacklists: string[]
}

export interface UpdateBlockingCommand {
  templateName: string
  blacklists: string[]
  id: string
}
