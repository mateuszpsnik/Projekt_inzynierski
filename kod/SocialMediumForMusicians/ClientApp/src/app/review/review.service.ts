import { Inject, Injectable } from "@angular/core";
import { HttpClient, HttpParams} from "@angular/common/http";
import { Observable } from "rxjs";
import { Review } from "./review";

@Injectable({ providedIn: "root" })
export class ReviewService {
    constructor(
        private http: HttpClient,
        @Inject("BASE_URL")
        private baseUrl: string) { }

    getReviews(top: number) : Observable<Array<Review>> {
        let url = this.baseUrl + "api/Reviews/";
        let params = new HttpParams().set("top", top.toString());

        return this.http.get<Array<Review>>(url, { params });
    }

    get(id): Observable<Review> {
        let url = this.baseUrl + "api/Reviews/" + id;
        return this.http.get<Review>(url);
    }

    put(review): Observable<Review> {
        let url = this.baseUrl + "api/Reviews/" + review.id;
        return this.http.put<Review>(url, review);
    }

    post(review: Review): Observable<Review> {
        let url = this.baseUrl + "api/Reviews/";
        return this.http.post<Review>(url, review);
    }
}