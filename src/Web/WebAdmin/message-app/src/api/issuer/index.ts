import { defHttp } from '/@/utils/http/axios'
import type { PagingFindResultWrapper } from '/@/api/model/baseModel'
import type { IssuerQuery, IssuerVM, AddIssuer, UpdateIssuer } from './model'

enum Api {
  getIssuerByPage = '/issuers',
  AddIssuer = '/issuers',
  updateIssuer = '/issuers',
}

export const getIssuerByPage = (params?: IssuerQuery) =>
  defHttp.get<PagingFindResultWrapper<IssuerVM>>({
    url: Api.getIssuerByPage,
    params,
  })

export const addIssuer = (params?: AddIssuer) => defHttp.post({ url: Api.AddIssuer, params })

export const updateIssuer = (params?: UpdateIssuer) =>
  defHttp.put({ url: Api.updateIssuer, params })
