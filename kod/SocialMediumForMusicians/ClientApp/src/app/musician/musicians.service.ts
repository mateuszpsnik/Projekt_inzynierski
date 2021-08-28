import { Inject, Injectable } from "@angular/core";
import { HttpClient, HttpParams } from "@angular/common/http";
import { Observable } from "rxjs";
import { Musician } from "./musician";

export interface PaginationApiResult<T> {
    elements: T[];
    totalCount: number;
    totalPages: number;
    pageIndex: number;
    pageSize: number;
}

@Injectable({ providedIn: "root" })
export class MusicianService {
    constructor(
        private http: HttpClient,
        @Inject("BASE_URL") 
        private baseUrl: string) { }

    getMusicians<PaginationApiResult>(pageIndex: number, pageSize: number)
            : Observable<PaginationApiResult> {
        let url = this.baseUrl + "api/Musicians/";
        let params = new HttpParams().set("pageIndex", pageIndex.toString())
                                     .set("pageSize", pageSize.toString());
        return this.http.get<PaginationApiResult>(url, { params });
    }

    get<Musician>(id): Observable<Musician> {
        let url = this.baseUrl + "api/Musicians/" + id;
        return this.http.get<Musician>(url);
    }

    put<Musician>(musician): Observable<Musician> {
        let url = this.baseUrl + "api/Musicians/" + musician.id;
        return this.http.put<Musician>(url, musician);
    }

    post<Musician>(musician: Musician): Observable<Musician> {
        let url = this.baseUrl + "api/Musicians/";
        return this.http.post<Musician>(url, musician);
    }
}