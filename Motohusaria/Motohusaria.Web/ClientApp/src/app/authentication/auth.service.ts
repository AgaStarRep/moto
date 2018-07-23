import { Injectable, Injector } from "@angular/core";
import * as jwtDecode from "jwt-decode";
import { HttpClient } from "@angular/common/http";

const STORAGE_KEY = "App-Token";
const ROLE_CLAIM = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
const NAME_CLAIM = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";
const ID_CLAIM = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";

import { Observable } from "rxjs";
@Injectable()
export class AuthService {
    private _httpClient: HttpClient
    private get httpClient() {
        if (!this._httpClient) {
            this._httpClient = this.injector.get(HttpClient);
        }
        return this._httpClient;
    }
    constructor(private injector: Injector) { }

    private decodedToken: any;

    get token(): string | null {
        if (this.isLoggedIn()) {
            return this.getTokenString();
        }
        return null;
    }

    get Roles(): string[] {
        return this.getTokenProperty<string[]>(ROLE_CLAIM);
    }

    get Id(): string {
        return this.getTokenProperty<string>(ID_CLAIM);
    }

    get Name(): string {
        return this.getTokenProperty<string>(NAME_CLAIM);
    }

    login(login: string, password: string, remember: boolean): Promise<boolean> {
        return this.httpClient.post("/account/token", { login, password })
            .toPromise()
            .then(data => {
                return this.storeToken(data, remember);
            })
            .catch(e => false)

    }

    register(login: string, password: string, passwordConfirm: string): Promise<boolean> {
        return this.httpClient.post("/account/register", { login, password, passwordConfirm })
            .toPromise()
            .then(data => {
                return this.storeToken(data, false);
            })
            .catch(e => false);
    }

    signout() {
        if (localStorage) {
            localStorage.removeItem(STORAGE_KEY);
        }
        if (sessionStorage) {
            sessionStorage.removeItem(STORAGE_KEY);
        }
        this.decodedToken = null;
    }

    storeToken(data: any, remember: boolean): boolean {
        if (data['result']) {
            if (remember && localStorage) {
                localStorage.setItem(STORAGE_KEY, data['result'])
            }
            else if (sessionStorage) {
                sessionStorage.setItem(STORAGE_KEY, data['result'])
            }
            return true;
        }
        return false;
    }

    private getToken(): any {
        if (this.decodedToken && this.decodedToken.exp < new Date().getTime()) {
            return this.decodedToken;
        }

        var token = this.getTokenString();
        if (token) {
            var decodedToken = jwtDecode(token);
            if (decodedToken.exp > (new Date().getTime() / 1000)) {
                this.decodedToken = decodedToken;
                return decodedToken;
            }
            else {
                this.signout()
            }
        }
        return null;
    }

    private getTokenString() {
        var token = "";
        if (localStorage) {
            token = localStorage.getItem(STORAGE_KEY);
        }
        if (!token && sessionStorage) {
            token = sessionStorage.getItem(STORAGE_KEY);
        }
        return token;
    }

    private getTokenProperty<T>(property: string): T | null {
        var token = this.getToken();
        if (token) {
            return token[property] as T;
        }
        return null;
    }

    isLoggedIn(): boolean {
        const token = this.getToken();
        if (token) {
            return true;
        } else {
            return false;
        }
    }
}
