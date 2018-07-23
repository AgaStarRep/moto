export interface TableResponse<T> {
    rows: T[],
    records: number,
    page: number,
    total: number;
}