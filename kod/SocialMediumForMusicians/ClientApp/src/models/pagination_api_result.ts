export interface PaginationApiResult<T> {
    elements: T[];
    totalCount: number;
    totalPages: number;
    pageIndex: number;
    pageSize: number;
}
