import { FormGroup, FormControl } from "@angular/forms";
import * as _moment from "moment";
import { isNumber } from "util";

export function markFormGroupTouched(formGroup: FormGroup) {
    if (formGroup.controls) {
        const keys = Object.keys(formGroup.controls);
        for (let i = 0; i < keys.length; i++) {
            const control = formGroup.controls[keys[i]];

            if (control instanceof FormControl) {
                control.markAsTouched();
                control.markAsDirty();
            } else if (control instanceof FormGroup) {
                this.markFormGroupTouched(control);
            }
        }
    }
}

/**
 * Formatuje objekt z momentjs do stringa YYYY-MM-DD
 * @param value data
 */
export function formatMomentJSDateToString(value: _moment.Moment | null | undefined) {
    if (value) {
        return value.format("YYYY-MM-DD");
    }
}

/**
 * Dla daty jako timestamp lub string w formacie "YYYY-MM-DD" zwraca obiekt momentJS z nadpisanym toJSON do zwracania dat w formacie "YYYY-MM-DD"
 * @param value data
 */
export function formatDateToMomentJsObj(value: string | number) {
    if (!value) {
        return;
    }
    if (isNumber(value)) {
        value = _moment(value).format("YYYY-MM-DD");
    }
    const newValue = _moment(value, "YYYY-MM-DD");
    newValue.toJSON = function () {
        return this.format("YYYY-MM-DD");
    };
    return newValue;
}


