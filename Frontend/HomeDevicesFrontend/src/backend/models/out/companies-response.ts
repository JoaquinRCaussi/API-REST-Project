import { CompanyResponse } from './company-response';

export interface CompaniesResponse {
  totalResults: number;
  pageNumber: number;
  pageSize: number;
  companies: CompanyResponse[];
}
