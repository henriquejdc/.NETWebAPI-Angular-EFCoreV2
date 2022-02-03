import { Component, OnInit, TemplateRef } from '@angular/core';
import { Router } from '@angular/router';

import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

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
  public eventosFiltrados: Evento[] = [];
  public eventoId = 0;
  public widthImg = 150;
  public marginImg = 2;
  public isCollapseImg = true;
  private filtroListado = '';

  public get filtroLista (): string {
    return this.filtroListado;
  }

  public set filtroLista(value: string){
    this.filtroListado = value;
    this.eventosFiltrados = this.filtroLista ? this.filtrarEventos(this.filtroLista) : this.eventos;
  }

  public filtrarEventos(filtrarPor: string): Evento[] {
    filtrarPor = filtrarPor.toLocaleLowerCase();
    return this.eventos.filter(
      (evento: { tema: string; local: string; }) =>
      evento.tema.toLocaleLowerCase().indexOf(filtrarPor) !== -1 ||
      evento.local.toLocaleLowerCase().indexOf(filtrarPor) !== -1
    );
  }

  constructor(
    private eventoService: EventoService,
    private modalService: BsModalService,
    private toastr: ToastrService,
    private spinner : NgxSpinnerService,
    private router: Router,
  ) { }

  public ngOnInit(): void {
    this.carregarEventos();
    this.spinner.show();
  }

  public carregarEventos(): void {
    const observer = {
      next: (eventosResp: Evento[]) => {
          this.eventos = eventosResp;
          this.eventosFiltrados = this.eventos;
        },
      error: (error: any) => {
        this.spinner.hide();
        this.toastr.error('Error ao Carregar', error);
      },
      complete: () => this.spinner.hide()
    }
    this.eventoService.getEvento().subscribe(observer);
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

}
