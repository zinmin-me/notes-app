export interface Note {
  id: string;
  title: string;
  content: string;
  createdAt: string;
  updatedAt: string;
}

export interface PaginatedResponse<T> {
  items: T[];
  currentPage: number;
  pageSize: number;
  totalPages: number;
  totalRecords: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
}

export type SortBy = 'CreatedAt' | 'UpdatedAt' | 'Title';

export type SortOrder = 'Ascending' | 'Descending';

export type DateFilter = 'All' | 'Today' | 'Last7Days' | 'Last30Days';

export interface NoteQueryParameters {
  page?: number;
  pageSize?: number;
  search?: string;
  sortBy?: SortBy;
  sortOrder?: SortOrder;
  dateFilter?: DateFilter;
}
