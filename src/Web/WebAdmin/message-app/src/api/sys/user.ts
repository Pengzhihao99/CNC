import { defHttp } from '/@/utils/http/axios'
import { LoginParams, LoginResultModel, GetUserInfoModel } from './model/userModel'

import { ErrorMessageMode } from '/#/axios'
import { useUserStore } from '/@/store/modules/user'

enum Api {
  Login = '/Users/login',
  Logout = '/Users/logout',
  GetUserInfo = '/Users/getUserInfo',
  GetPermCode = '/Users/getPermCode',
}

/**
 * @description: user login api
 */
export function loginApi(params: LoginParams, mode: ErrorMessageMode = 'modal') {
  return defHttp.post<LoginResultModel>(
    {
      url: Api.Login,
      params,
    },
    {
      errorMessageMode: mode,
    }
  )
}

/**
 * @description: getUserInfo
 */
export function getUserInfo() {
  const userStore = useUserStore()
  const token = userStore.getToken
  return defHttp.get<GetUserInfoModel>(
    { url: Api.GetUserInfo, params: { token } },
    { errorMessageMode: 'none' }
  )
}

export function getPermCode() {
  const userStore = useUserStore()
  const token = userStore.getToken
  return defHttp.get<string[]>({ url: Api.GetPermCode, params: { token } })
}

export function doLogout() {
  const userStore = useUserStore()
  const token = userStore.getToken
  return defHttp.get({ url: Api.Logout, params: { token } })
}
