
import { Component,OnInit,ViewChild,ElementRef } from '@angular/core';
import { ActivatedRoute,Router } from '@angular/router';
import { DatePipe }  from '@angular/common';

import { RoleService } from './role.service';
import { RoleListModel } from './role-list-model';
import { TableRequest } from '../common/table-request-model';
import { NavigationEnd } from '@angular/router/src/events';
import { RoleFiltersComponent } from './role-filters.component';


@Component({
    templateUrl: './role-list.component.html',
    host: {'class': 'entity-list-page'},
})
export class RoleListComponent implements OnInit {
    constructor(private service: RoleService, private route: ActivatedRoute, private router: Router ) { }
    entities: RoleListModel[];
    @ViewChild('filters') filtersComponent: RoleFiltersComponent;
    @ViewChild('tableContainer', { read: ElementRef }) table: ElementRef;
    displayedColumns = ['name']
    tableRequest: TableRequest
    pagesToScroll: number = 1;
    async ngOnInit()
        {
            this.service.DataChangedEvent.subscribe(next => this.loadData(true).then(t => this.entities = t.rows));
            this.route.queryParamMap.subscribe(next => {
                this.tableRequest = {
                    page: parseInt(next.get('page') || '1'),
                rows: parseInt(next.get('rows') || '20'),
                sidx: next.get('sidx') || '',
                sord: next.get('sord') || 'asc'
                }
                var parms = next.keys.reduce((a, b) => {
                    a[b] = next.get(b);
                    return a;
                }, {});
                this.filtersComponent.setFilters(parms);
                this.loadData(true).then(data => {
                    this.keepLoadingUntilScrollbar(data.rows, data.records)
                });
            });
        }

        async loadData(includePrevousPages: boolean)
        {
            var request = Object.assign({}, this.tableRequest, {includePrevousPages});
            const entities = await this.service.search(request, this.filtersComponent.filters);
            return entities;
        }

        filter(){
            this.reload(true);
        }

        private reload(resetScroll : boolean){
            this.router.navigate(["role"], {
                queryParams: Object.assign({ }, this.tableRequest, this.filtersComponent.filters)
            });
            if(resetScroll){
                this.table.nativeElement.scrollTop = 0;
            }
        }

        sortData(event){
            if(event.direction){
                this.tableRequest.sidx = event.active
                this.tableRequest.sord = event.direction;
            }
            else{
                this.tableRequest.sidx = '';
                this.tableRequest.sord = '';
            }

            this.tableRequest.page = this.pagesToScroll;
            this.reload(true);
        }

        private async keepLoadingUntilScrollbar(entities: RoleListModel[], total : number){
            this.entities = entities;
            var hasScroll = () => this.table.nativeElement.scrollHeight > this.table.nativeElement.clientHeight;
            while(!hasScroll() && this.entities.length != total){
                this.tableRequest.page++;
                const data = await this.loadData(false);
                this.entities = this.entities.concat(data.rows);
            }
            this.pagesToScroll = this.tableRequest.page;
        }

        async onScroll(){
            this.tableRequest.page++;
            this.router.navigate(["role"], {
                queryParams: Object.assign({ }, this.tableRequest, this.filtersComponent.filters)
            });
        }
    }
            
