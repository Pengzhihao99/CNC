import type { App } from 'vue'
import { Button } from './Button'
import {
  // Need
  Button as AntButton,
  Layout,
  Input,
  Select,
  Alert,
  Checkbox,
  DatePicker,
  Radio,
  Switch,
  Card,
  List,
  Tabs,
  Descriptions,
  Tree,
  Table,
  Divider,
  Modal,
  Drawer,
  Dropdown,
  Tag,
  Tooltip,
  Badge,
  Popover,
  Upload,
  Transfer,
  Steps,
  PageHeader,
  Result,
  Empty,
  Avatar,
  Menu,
  Breadcrumb,
  Form,
  Row,
  Col,
  Spin,
  Pagination,
} from 'ant-design-vue'

const compList = [AntButton.Group]

export function registerGlobComp(app: App) {
  compList.forEach(comp => {
    app.component(comp.name || comp.displayName, comp)
  })

  app
    .use(Input)
    .use(Button)
    .use(Layout)
    .use(Select)
    .use(Alert)
    .use(Breadcrumb)
    .use(Checkbox)
    .use(DatePicker)
    .use(Radio)
    .use(Switch)
    .use(Card)
    .use(List)
    .use(Descriptions)
    .use(Tree)
    .use(Table)
    .use(Divider)
    .use(Modal)
    .use(Drawer)
    .use(Dropdown)
    .use(Tag)
    .use(Tooltip)
    .use(Badge)
    .use(Popover)
    .use(Upload)
    .use(Transfer)
    .use(Steps)
    .use(PageHeader)
    .use(Result)
    .use(Empty)
    .use(Avatar)
    .use(Menu)
    .use(Tabs)
    .use(Form)
    .use(Row)
    .use(Col)
    .use(Spin)
    .use(Pagination)
}
