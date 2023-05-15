import type { AppRouteModule } from '/@/router/types'

import { LAYOUT } from '/@/router/constant'
import { t } from '/@/hooks/web/useI18n'

const dashboard: AppRouteModule = {
  path: '/template',
  name: 'Template',
  component: LAYOUT,
  redirect: '/template/index',
  meta: {
    hideChildrenInMenu: true,
    icon: 'ri:bill-fill',
    //title: t('routes.dashboard.messageSender'),
    title: '消息模板管理',
    orderNo: 1,
  },
  children: [
    {
      path: 'index',
      name: 'MessageTemplatePage',
      component: () => import('/@/views/templates/index.vue'),
      meta: {
        title: '消息模板管理',
        icon: 'simple-icons:about-dot-me',
        hideMenu: true,
      },
    },
  ],
}

export default dashboard
