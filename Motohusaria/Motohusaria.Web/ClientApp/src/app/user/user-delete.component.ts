
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { UserService } from './user.service';

@Component({
    templateUrl:'./user-delete.component.html'
})
export class UserDeleteComponent implements OnInit {
    constructor(private service: UserService, private route : ActivatedRoute, private router: Router) { }

    id: string;
    isDeleting: boolean = false;

    ngOnInit(){
         this.route.paramMap.subscribe(next => {
            let id = next.get('id');
            if(id){
                this.id = id;
            }
         });
    }

    async ok(){
        if(!this.isDeleting){
            this.isDeleting = true;
            const result = await this.service.delete(this.id);
            this.isDeleting = false; 
            if(result){
                this.return();
            }
            //todo notify error
        }
    }

    return(){
        this.router.navigate(['user'], { queryParams: this.route.snapshot.queryParams, replaceUrl: true });        
    }
}

