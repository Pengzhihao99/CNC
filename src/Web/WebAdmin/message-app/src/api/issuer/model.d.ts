import { PagingFindQuery } from '/@/api/model/baseModel'

export interface IssuerVM {
  id: string
  name: string
  token: string
  enabled: boolean
  remark: string
  updateOn: string
}

export interface AddIssuer {
  id?: string
  name: string
  enabled: boolean
  remark: string
}

export interface UpdateIssuer {
  id?: string
  enabled: boolean
  remark: string
}

export interface FormStateForSearch {
  name: string
}

export interface IssuerQuery extends PagingFindQuery {
  name?: string
  //enabled?: string
}
