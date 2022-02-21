import { Component, OnInit, TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { PaginatedResult, Pagination } from '@app/models/Pagination';
import { environment } from '@environments/environment';

import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { debounce, debounceTime, Subject } from 'rxjs';

import { Evento } from '../../../models/Evento';
import { EventoService } from '../../../services/evento.service';

@Component({
  selector: 'app-evento-lista',
  templateUrl: './evento-lista.component.html',
  styleUrls: ['./evento-lista.component.scss']
})
export class EventoListaComponent implements OnInit {

  modalRef?: BsModalRef;
  message?: string;

  public eventos: Evento[] = [];
  public eventoId = 0;
  public widthImg = 150;
  public marginImg = 2;
  public isCollapseImg = true;
  public pagination = {} as Pagination;

  termoBuscaChanged: Subject<string> = new Subject<string>();

  public filtrarEventos(event: any): void {
    if (this.termoBuscaChanged.observers.length == 0)
    {
      this.termoBuscaChanged.pipe(debounceTime(1000)).subscribe(
        filtratPor =>
        {
          this.spinner.show();
          this.eventoService
              .getEventos(this.pagination.currentPage,
                          this.pagination.itemsPerPage,
                          event.value
              ).subscribe(
                (paginatedResult: PaginatedResult<Evento[]>) => {
                  this.eventos = paginatedResult.result;
                  this.pagination = paginatedResult.pagination;
                },
                (error: any) => {
                  this.toastr.error('Erro ao Carregar os Eventos', 'Erro!');
                }
              ).add(() => this.spinner.hide())
        }
      )
    }
    this.termoBuscaChanged.next(event.value);
  }

  constructor(
    private eventoService: EventoService,
    private modalService: BsModalService,
    private toastr: ToastrService,
    private spinner : NgxSpinnerService,
    private router: Router,
  ) { }

  public ngOnInit(): void {
    this.pagination = {currentPage:1, itemsPerPage: 1, totalItems: 1 } as Pagination;
    this.carregarEventos();
  }

  public carregarEventos(): void {
    this.spinner.show();

    this.eventoService
      .getEventos(this.pagination.currentPage, this.pagination.itemsPerPage)
      .subscribe(
        (paginatedResult: PaginatedResult<Evento[]>) => {
          this.eventos = paginatedResult.result;
          this.pagination = paginatedResult.pagination;
        },
        (error: any) => {
          this.toastr.error('Erro ao Carregar os Eventos', 'Erro!');
        }
      )
      .add(() => this.spinner.hide());
  }


  openModal(event: any, template: TemplateRef<any>, eventoId: number) : void {
    event.stopPropagation();
    this.eventoId = eventoId;
    this.modalRef = this.modalService.show(template, {class: 'modal-sm'});
  }

  confirm(): void {
    this.modalRef?.hide();
    this.spinner.show();

    this.eventoService.deleteEvento(this.eventoId).subscribe(
      (result: any) => {
          this.toastr.success('Evento deletado com sucesso!', result);
          this.carregarEventos();
      },
      (error: any) => {
        this.toastr.error(`Erro ao deletar evento ${this.eventoId} !`, 'Não deletado!');
      }
    ).add(() => this.spinner.hide());

    this.toastr.success('Evento deletado com sucesso!','Deletado!');
  }

  decline(): void {
    this.modalRef?.hide();
  }

  detalheEvento(id: number): void {
    this.router.navigate([`eventos/detalhe/${id}`]);
  }

  mostraImagem(imagemURL: string): string {
    return (imagemURL != '') ? `${environment.api}resources/images/${imagemURL}` : 'assets/cloud.png'
  }

  public pageChanged(event): void {
    this.pagination.currentPage = event.page;
    this.carregarEventos();
  }

}
