import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable()
export class RequestProgressBarService {
    private _visable : Subject<boolean> = new Subject<boolean>();
    public get visable(){return this._visable.asObservable();}

    public setVisbility(val: boolean){
        this._visable.next(val);
    }
}