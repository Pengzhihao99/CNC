import { defHttp } from '/@/utils/http/axios'
import type { PagingFindResultWrapper } from '/@/api/model/baseModel'
import type {
  AddSendingService,
  SendingServiceQuery,
  SendingServiceVM,
  UpdateSendingService,
} from '/@/api/sendingService/model'

enum Api {
  getSendingServiceByPage = '/sendingServices',
  AddSendingService = '/sendingServices',
  updateSendingService = '/sendingServices',
}

export const getSendingServicesByPage = (params?: SendingServiceQuery) =>
  defHttp.get<PagingFindResultWrapper<SendingServiceVM>>({
    url: Api.getSendingServiceByPage,
    params,
  })

export const addSendingService = (params: AddSendingService) =>
  defHttp.post({ url: Api.AddSendingService, params })

export const updateSendingService = (params: UpdateSendingService) =>
  defHttp.put({ url: Api.updateSendingService, params })
