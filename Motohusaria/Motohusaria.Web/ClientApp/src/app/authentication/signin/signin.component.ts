import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import {
    FormBuilder,
    FormGroup,
    Validators,
    FormControl
} from "@angular/forms";
import { AuthService } from "../auth.service";

@Component({
    selector: "app-signin",
    templateUrl: "./signin.component.html",
    styleUrls: ["./signin.component.scss"]
})
export class SigninComponent implements OnInit {
    public form: FormGroup;
    constructor(
        private fb: FormBuilder,
        private router: Router,
        private authService: AuthService
    ) {}

    ngOnInit() {
        this.form = this.fb.group({
            uname: [null, Validators.compose([Validators.required])],
            password: [null, Validators.compose([Validators.required])],
            remember : [false, null]
        });
    }

    onSubmit() {
        if (this.form.invalid) {
            return;
        }
        const form = this.form.value;
        this.authService.login(form.uname, form.password, form.remember)
          .then(success => {
            if(success){
              this.router.navigate(["/"]);              
            }
        });
    }
}
