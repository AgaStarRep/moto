import { Platform } from "@angular/cdk/platform";
import { Inject, Injectable, Optional } from "@angular/core";
import { DateAdapter, MAT_DATE_LOCALE } from "@angular/material";
import { MomentDateAdapter } from '@angular/material-moment-adapter'

import * as _moment from "moment";
const moment = _moment;


@Injectable()
export class LocalDateAdapter extends MomentDateAdapter  {
    createDate(year: number, month: number, date: number): _moment.Moment {
        // Moment.js will create an invalid date if any of the components are out of bounds, but we
        // explicitly check each case so we can throw more descriptive errors.
        if (month < 0 || month > 11) {
          throw Error(`Invalid month index "${month}". Month index has to be between 0 and 11.`);
        }
    
        if (date < 1) {
          throw Error(`Invalid date "${date}". Date has to be greater than 0.`);
        }
    
        let result = moment({year, month, date}).locale(this.locale);
    
        // If the result isn't valid, the date must have been out of bounds for this month.
        if (!result.isValid()) {
          throw Error(`Invalid date "${date}" for month with index "${month}".`);
        }
        this.changeMomentToJson(result);
        return result;
      }

      clone(value: _moment.Moment){
        const cloned = super.clone(value);
        this.changeMomentToJson(cloned);
        return cloned;
      }
    
      private changeMomentToJson(value: _moment.Moment){
         value.toJSON = function(){
             return this.format("YYYY-MM-DD");
         }
      }
}
