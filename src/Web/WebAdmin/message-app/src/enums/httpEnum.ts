/**
 * @description: Request result set
 */
export enum ResultEnum {
  SUCCESS = 0,
  ERROR = 1,
  TIMEOUT = 401,
  TYPE = 'success',
}

/**
 * @description: request method
 */
export enum RequestEnum {
  GET = 'GET',
  POST = 'POST',
  PUT = 'PUT',
  DELETE = 'DELETE',
}

/**
 * @description:  contentTyp
 */
export enum ContentTypeEnum {
  // json
  JSON = 'application/json',
  // form-data qs
  FORM_URLENCODED = 'application/x-www-form-urlencodedcharset=UTF-8',
  // form-data  upload
  FORM_DATA = 'multipart/form-datacharset=UTF-8',
}
