import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';

export interface PuzzleLog {
  time: string;
  name: string;
  ip: string;
  msg: string;
  status: string;
}

export interface GetLogsParams {
  skip: number;
  count: number;
}

export const ADMIN_API = createApi({
  reducerPath: 'adminApi',
  baseQuery: fetchBaseQuery({ baseUrl: 'api/admin' }),
  endpoints: (builder) => ({
    getLogs: builder.query<PuzzleLog[], GetLogsParams>({
      query: (params) => ({
        url: 'logs',
        params
      })
    })
  })
});
