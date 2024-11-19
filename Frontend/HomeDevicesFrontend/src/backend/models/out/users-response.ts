import { UserResponse } from './user-response';

export interface UsersResponse {
  totalResults: number;
  pageNumber: number;
  pageSize: number;
  users: UserResponse[];
}
