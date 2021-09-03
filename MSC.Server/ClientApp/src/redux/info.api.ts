import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';

export interface PuzzleScoreBoard {
  updateTime: string;
  rank: {
    score: number;
    updateTime: string;
    userName: string;
    descr: string;
    userId: string;
  }[];
  topDetail: {
    userName: string;
    timeLine: {
      puzzleId: string;
      time: string;
      totalScore: number;
    }[];
  }[];
}

export const INFO_API = createApi({
  reducerPath: 'infoApi',
  baseQuery: fetchBaseQuery({ baseUrl: 'Info' }),
  endpoints: (builder) => ({
    getScoreBoard: builder.query<PuzzleScoreBoard, void>({
      query: () => 'ScoreBoard'
    })
  })
});
