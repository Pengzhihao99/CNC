import { defHttp } from '/@/utils/http/axios'
import type {
  BlockingQuery,
  BlockingVM,
  CreateBlockingCommand,
  UpdateBlockingCommand,
} from '/@/api/blocking/model'
import type { PagingFindResultWrapper } from '/@/api/model/baseModel'

enum Api {
  mainBlockingApi = '/blockings',
}

export const getBlockingByPage = (params?: BlockingQuery) =>
  defHttp.get<PagingFindResultWrapper<BlockingVM>>({
    url: Api.mainBlockingApi,
    params,
  })

export const createBlocking = (data: CreateBlockingCommand) =>
  defHttp.post({
    url: Api.mainBlockingApi,
    data,
  })

export const updateBlocking = (data: UpdateBlockingCommand) =>
  defHttp.put<PagingFindResultWrapper<BlockingVM>>({
    url: Api.mainBlockingApi + '/' + data.id,
    data,
  })
