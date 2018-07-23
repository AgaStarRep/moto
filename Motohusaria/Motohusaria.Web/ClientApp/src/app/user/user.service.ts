import { Injectable, Inject } from '@angular/core';
import { HttpClientModule, HttpClient, HttpParams } from '@angular/common/http';

import { Subject ,  Observable } from 'rxjs';
import { LookupModel } from '../common/lookup-model';
import { ServiceResponse } from '../common/service-response-model'

import { UserListModel } from './user-list-model';
import { UserModel } from './user-model';
import { TableRequest } from '../common/table-request-model';
import { TableResponse } from '../common/table-response-mode';
import { OptionsModel } from '../common/options-model';

@Injectable({ providedIn: 'root' })
export class UserService {
    constructor(private http: HttpClient) {
    }
    private _dataChangedEvent = new Subject();

    get DataChangedEvent(): Observable<any> {
        return this._dataChangedEvent.asObservable();
    }

    async search(tableRequest: TableRequest, filter?: UserListModel): Promise<TableResponse<UserListModel>> {
        let params = new HttpParams();
        var keys = Object.keys(tableRequest);
        function addParams(paramsObj: any) {
            var keys = Object.keys(paramsObj);
            for (let i = 0; i < keys.length; i++) {
                if (paramsObj[keys[i]] != null) {
                    params = params.set(keys[i], paramsObj[keys[i]]);
                }
            }
        }
        addParams(tableRequest);
        if (filter) {
            addParams(filter);
        }
        const response = await this.http.get<ServiceResponse<TableResponse<UserListModel>>>(`/api/User`, { params }).toPromise();
        return response.result;
    }

    async getOptions(search?: string | null): Promise<LookupModel[]> {
        const response = await this.http.get(`/api/User/lookup?search=${search || ''}`).toPromise() as ServiceResponse<OptionsModel>;
        return response.result.results.map(m => { return { label: m.text, value: m.id.toLowerCase() } }) as LookupModel[];
    }

    async getById(id: string): Promise<UserModel> {
        const response = await this.http.get<ServiceResponse<UserModel>>(`/api/User/${id}`).toPromise();
        return response.result;
    }

    async delete(id: string): Promise<ServiceResponse> {
        const response = await this.http.delete<ServiceResponse>(`/api/User/${id}`).toPromise();
        this._dataChangedEvent.next();
        return response;
    }

    async add(model: UserModel): Promise<ServiceResponse> {
        const response = await this.http.post<ServiceResponse>(`/api/User/`, model).toPromise();
        this._dataChangedEvent.next();
        return response;
    }

    async update(model: UserModel): Promise<ServiceResponse> {
        const response = await this.http.put<ServiceResponse>(`/api/User/${model.id}`, model).toPromise();
        this._dataChangedEvent.next();
        return response;
    }
}
