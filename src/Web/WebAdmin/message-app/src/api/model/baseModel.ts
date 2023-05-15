export interface BasicPageParams {
  page: number
  pageSize: number
}

export interface BasicFetchResult<T> {
  items: T[]
  total: number
}

export interface PagingFindQuery {
  pageNumber?: number
  pageSize?: number
  paging?: boolean
}

export interface PagingFindResultWrapper<T> {
  totalRecords: number
  totalPages: number
  pageNumber: number
  pageSize: number
  data: T[]
}
