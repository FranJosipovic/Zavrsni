export interface HttpResponseModel<T> {
  statusCode: number;
  isSuccess: boolean;
  data: T | null;
  message: string;
}

export interface ResponseCollection<T>{
  items: T[],
  count: number
}