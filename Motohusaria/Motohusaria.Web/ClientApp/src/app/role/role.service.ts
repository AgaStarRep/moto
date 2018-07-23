import { Injectable, Inject } from '@angular/core';
import { HttpClientModule, HttpClient, HttpParams } from '@angular/common/http';

import { Subject ,  Observable } from 'rxjs';
import { LookupModel } from '../common/lookup-model';
import { ServiceResponse } from '../common/service-response-model'

import { RoleListModel } from './role-list-model';
import { RoleModel } from './role-model';
import { TableRequest } from '../common/table-request-model';
import { TableResponse } from '../common/table-response-mode';
import { OptionsModel } from '../common/options-model';

@Injectable({ providedIn: 'root' })
export class RoleService {
    constructor(private http: HttpClient) {
    }
    private _dataChangedEvent = new Subject();

    get DataChangedEvent(): Observable<any> {
        return this._dataChangedEvent.asObservable();
    }

    async search(tableRequest: TableRequest, filter?: RoleListModel): Promise<TableResponse<RoleListModel>> {
        let params = new HttpParams();
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
        const response = await this.http.get<ServiceResponse<TableResponse<RoleListModel>>>(`/api/Role`, { params }).toPromise();
        return response.result;
    }

    async getOptions(search?: string | null): Promise<LookupModel[]> {
        const response = await this.http.get(`/api/Role/lookup?search=${search || ''}`).toPromise() as ServiceResponse<OptionsModel>;
        return response.result.results.map(m => { return { label: m.text, value: m.id.toLowerCase() } }) as LookupModel[];
    }

    async getById(id: string): Promise<RoleModel> {
        const response = await this.http.get<ServiceResponse<RoleModel>>(`/api/Role/${id}`).toPromise();
        return response.result;
    }

    async delete(id: string): Promise<ServiceResponse> {
        const response = await this.http.delete<ServiceResponse>(`/api/Role/${id}`).toPromise();
        this._dataChangedEvent.next();
        return response;
    }

    async add(model: RoleModel): Promise<ServiceResponse> {
        const response = await this.http.post<ServiceResponse>(`/api/Role/`, model).toPromise();
        this._dataChangedEvent.next();
        return response;
    }

    async update(model: RoleModel): Promise<ServiceResponse> {
        const response = await this.http.put<ServiceResponse>(`/api/Role/${model.id}`, model).toPromise();
        this._dataChangedEvent.next();
        return response;
    }
}
