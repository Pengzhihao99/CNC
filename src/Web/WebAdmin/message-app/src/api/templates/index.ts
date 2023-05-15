import { defHttp } from '/@/utils/http/axios'
import type { PagingFindResultWrapper } from '/@/api/model/baseModel'
import type {
  TemplateVM,
  SendingServiceNamesVM,
  CreateTemplateCommand,
  TemplateQuery,
  TestTemplateCommand,
  UpdateTemplateCommand,
} from './model'

enum Api {
  getTemplateByPage = '/templates',
  AddTemplate = '/templates',
  updateTemplate = '/templates',
  testTemplate = '/templates/test',
  getSendingServiceNames = '/sendingServices/serviceNames',
}

export const getTemplateByPage = (params?: TemplateQuery) =>
  defHttp.get<PagingFindResultWrapper<TemplateVM>>({
    url: Api.getTemplateByPage,
    params,
  })

export const getSendingServiceNames = () =>
  defHttp.get<SendingServiceNamesVM[]>({
    url: Api.getSendingServiceNames,
  })

export const testTemplate = (params?: TestTemplateCommand) =>
  defHttp.post({ url: Api.testTemplate, params })

export const addTemplate = (params?: CreateTemplateCommand) =>
  defHttp.post({ url: Api.AddTemplate, params })

export const updateTemplate = (params?: UpdateTemplateCommand) =>
  defHttp.put({ url: Api.updateTemplate, params })
