import { Inject, Injectable } from "@angular/core";
import { HttpClient, HttpParams } from "@angular/common/http";
import { Observable } from "rxjs";
import { User } from "src/models/User";

@Injectable({ providedIn: 'root' })
export class UserService {
    constructor(
        private http: HttpClient,
        @Inject('BASE_URL')
        private baseUrl: string) {}

    get(id: string): Observable<User> {
        const url = this.baseUrl + 'api/Users/' + id;
        return this.http.get<User>(url);
    }

    put(user: User): Observable<User> {
        const url = this.baseUrl + 'api/Users/' + user.id;
        return this.http.put<User>(url, user);
    }

    lockout(userId: string, adminId: string, end: Date): Observable<User> {
        const url = this.baseUrl + 'api/Lockout/' + userId;
        const params = new HttpParams().set('adminId', adminId)
                                       .set('end', end.toUTCString());
        return this.http.get<User>(url, { params });
    }
}
