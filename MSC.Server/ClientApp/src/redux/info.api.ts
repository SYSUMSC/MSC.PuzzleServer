import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';

export interface PuzzleScoreBoard {
  updateTime: string;
  rank: {
    score: number;
    time: string;
    name: string;
    descr: string;
  }[];
  topDetail: {
    userName: string;
    timeLine: {
      time: string;
      score: number;
    }[];
  }[];
}

export const INFO_API = createApi({
  reducerPath: 'infoApi',
  baseQuery: fetchBaseQuery({ baseUrl: 'api/info' }),
  refetchOnFocus: true,
  refetchOnReconnect: true,
  refetchOnMountOrArgChange: true,
  endpoints: (builder) => ({
    getScoreBoard: builder.query<PuzzleScoreBoard, void>({
      query: () => 'scoreboard'
    })
  })
});
