import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import {
    FormBuilder,
    FormGroup,
    Validators,
    FormControl,
    Validator
} from "@angular/forms";
import { AuthService } from "../auth.service";
import { matchingPasswords } from '../../common/validators/compare-password.validator';

@Component({
    selector: "app-signup",
    templateUrl: "./signup.component.html",
    styleUrls: ["./signup.component.scss"]
})
export class SignupComponent implements OnInit {
    public form: FormGroup;
    constructor(
        private fb: FormBuilder,
        private router: Router,
        private authService: AuthService
    ) {}

    ngOnInit() {
        this.form = this.fb.group({
            uname: [null, Validators.compose([Validators.required])],
            password: [null, Validators.required],
            confirmPassword: [null, Validators.required]
        }, {validator: matchingPasswords('password','confirmPassword') });
    }

    onSubmit() {
        if (this.form.invalid) {
            return;
        }
        var value = this.form.value;
        this.authService
            .register(value.uname, value.password, value.confirmPassword)
            .then(success => {
                if(success){
                    this.router.navigate(["/"]);                    
                }
            });
    }
}
