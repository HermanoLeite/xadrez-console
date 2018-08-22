using System;
using System.Collections.Generic;
using tabuleiro;
namespace xadrez {
    class PartidaDeXadrez {
        public Tabuleiro tab { get; private set; }
        public int turno { get; private set; } 
        public Cor jogadorAtual { get; private set; }
        public Boolean terminada;
        private HashSet<Peca> pecas;
        private HashSet<Peca> capturadas;
        public Boolean xeque { get; private set; }
        public PartidaDeXadrez () {
            tab = new Tabuleiro(8,8);
            turno = 1;
            jogadorAtual = Cor.Branca;
            terminada = false;
            pecas = new HashSet<Peca>();
            capturadas = new HashSet<Peca>();
            xeque = false;
            colocarPecas();
        }
        public void validarPosicaoDeDestino(Posicao origem, Posicao destino) {
            if(!tab.peca(origem).movimentoPossivel(destino)) {
                throw new TabuleiroException("Posição de destino inválida");
            }
        }
        public void validarPosicaoDeOrigem(Posicao origem) {
            if(tab.peca(origem) == null) {
                throw new TabuleiroException("Não existe peça na posição de origem escolhida!");
            }
            if(tab.peca(origem).cor != jogadorAtual) {
                throw new TabuleiroException("A peça de origem escolhida não é a sua!");
            }
            if(!tab.peca(origem).existeMovimentosPossiveis()) {
                throw new TabuleiroException("Não há movimentos possíveis para a peça de origem escolhida!");
            }
        }
        private Cor adversaria(Cor cor) {
            if (cor == Cor.Branca) {
                return Cor.Preta;
            }
            else {
                return Cor.Branca;
            }
        }
        private Peca rei(Cor cor) {
            foreach (var item in pecasEmJogo(cor))
            {
                if(item is Rei) {
                    return item;
                }
            }
            throw new TabuleiroException("Jogo não tem rei!");
        }
        public bool estaEmXeque(Cor cor) {
            Peca r = rei(cor);
            foreach (var item in pecasEmJogo(adversaria(cor)))
            {
                var mat = item.movimentosPossiveis(); 
                if(mat[r.posicao.linha, r.posicao.coluna]) {
                    return true;
                }
            }
            return false;
        }
        public HashSet<Peca> pecasCapturadas(Cor cor) {
            HashSet<Peca> capturadasCor = new HashSet<Peca>();
            foreach (Peca capturada in capturadas) {
                if(capturada.cor == cor) {
                    capturadasCor.Add(capturada);
                }
            }
            return capturadasCor;
        }
        public HashSet<Peca> pecasEmJogo(Cor cor) {
            HashSet<Peca> pecasDaCor = new HashSet<Peca>();
            foreach (Peca item in pecas) {
                if(item.cor == cor) {
                    pecasDaCor.Add(item);
                }
            }
            pecasDaCor.ExceptWith(pecasCapturadas(cor));
            return pecasDaCor;
        }
        public Peca executaMovimento(Posicao origem, Posicao destino) {
            Peca p = tab.retirarPeca(origem);
            p.incrementarQtdMovimentos();
            Peca pecaCapturada = tab.retirarPeca(destino);
            tab.colocarPeca(p, destino);
            if(pecaCapturada != null) {
                capturadas.Add(pecaCapturada);
            }

            // #jogada especial roque pequeno
            if (p is Rei && destino.coluna == origem.coluna+2) {
                Posicao origemDaTorre = new Posicao (origem.linha, origem.coluna+3);
                Posicao destinoDaTorre = new Posicao (origem.linha, origem.coluna+1);
                Peca T = tab.retirarPeca(origemDaTorre);
                T.incrementarQtdMovimentos();
                tab.colocarPeca(T, destinoDaTorre);
            }
            
            // #jogada especial roque grande
            if (p is Rei && destino.coluna == origem.coluna-2) {
                Posicao origemDaTorre = new Posicao (origem.linha, origem.coluna-4);
                Posicao destinoDaTorre = new Posicao (origem.linha, origem.coluna-1);
                Peca T = tab.retirarPeca(origemDaTorre);
                T.incrementarQtdMovimentos();
                tab.colocarPeca(T, destinoDaTorre);
            }

            return pecaCapturada;
        }

        public bool testaXequemate (Cor cor) {
            if(!estaEmXeque(cor)) {
                return false;
            }
            foreach (var item in pecasEmJogo(cor))
            {
                bool[,] mat = item.movimentosPossiveis();
                for (int i = 0; i < tab.linhas; i++) {
                    for (int j = 0; j < tab.colunas; j++) {
                        if(mat[i,j]) {
                            var origem = item.posicao;
                            var destino = new Posicao(i, j);
                            Peca pecaCapturada = executaMovimento(origem, destino);
                            var testaXeque = estaEmXeque(cor);
                            desfazMovimento(origem, destino, pecaCapturada);
                            if(!estaEmXeque(cor)) {
                                return false;
                            } 
                        }
                    }                
                }
            }
            return true;
        }

        public void realizaJogada(Posicao origem, Posicao destino) {
            Peca pecaCapturada = executaMovimento(origem, destino);
            if(estaEmXeque(jogadorAtual)) {
                desfazMovimento(origem, destino, pecaCapturada);
                throw new TabuleiroException("Você não pode se colocar em xaque!");
            }
            xeque = false;
            if(estaEmXeque(adversaria(jogadorAtual))) {
                xeque = true;
            }
            if(testaXequemate(adversaria(jogadorAtual))) {
                terminada = true;
            }
            else {
                turno++;
                mudaJogador();
            }
        }
        public void desfazMovimento(Posicao origem, Posicao destino, Peca pecaCapturada) {
            Peca p = tab.retirarPeca(destino);
            p.decrementarQtdMovimentos();
            if (pecaCapturada != null) {
                tab.colocarPeca(pecaCapturada, destino);
                capturadas.Remove(pecaCapturada);
            }

            // #jogada especial roque pequeno
            if (p is Rei && destino.coluna == origem.coluna+2) {
                Posicao origemDaTorre = new Posicao (origem.linha, origem.coluna+3);
                Posicao destinoDaTorre = new Posicao (origem.linha, origem.coluna+1);
                Peca T = tab.retirarPeca(destinoDaTorre);
                T.decrementarQtdMovimentos();
                tab.colocarPeca(T, origemDaTorre);
            }
            
            // #jogada especial roque grande
            if (p is Rei && destino.coluna == origem.coluna-2) {
                Posicao origemDaTorre = new Posicao (origem.linha, origem.coluna-4);
                Posicao destinoDaTorre = new Posicao (origem.linha, origem.coluna-1);
                Peca T = tab.retirarPeca(destinoDaTorre);
                T.decrementarQtdMovimentos();
                tab.colocarPeca(T, origemDaTorre);
            }

            tab.colocarPeca(p, origem);
        }
        public void mudaJogador() {
            if(jogadorAtual == Cor.Branca) {
                jogadorAtual = Cor.Preta;
            }
            else {
                jogadorAtual = Cor.Branca;
            }
        }

        public void colocarNovaPeca (char coluna, int linha, Peca peca) {
            tab.colocarPeca(peca, new PosicaoXadrez(coluna, linha).toPosicao());
            pecas.Add(peca);
        }
        private void colocarPecas() {
            colocarNovaPeca('a', 1, new Torre(tab, Cor.Branca));
            colocarNovaPeca('b', 1, new Cavalo(tab, Cor.Branca));
            colocarNovaPeca('c', 1, new Bispo(tab, Cor.Branca));
            colocarNovaPeca('d', 1, new Dama(tab, Cor.Branca));
            colocarNovaPeca('e', 1, new Rei(tab, Cor.Branca, this));
            colocarNovaPeca('f', 1, new Bispo(tab, Cor.Branca));
            colocarNovaPeca('g', 1, new Cavalo(tab, Cor.Branca));
            colocarNovaPeca('h', 1, new Torre(tab, Cor.Branca));
            
            colocarNovaPeca('a', 2, new Peao(tab, Cor.Branca));
            colocarNovaPeca('b', 2, new Peao(tab, Cor.Branca));
            colocarNovaPeca('c', 2, new Peao(tab, Cor.Branca));
            colocarNovaPeca('d', 2, new Peao(tab, Cor.Branca));
            colocarNovaPeca('e', 2, new Peao(tab, Cor.Branca));
            colocarNovaPeca('f', 2, new Peao(tab, Cor.Branca));
            colocarNovaPeca('g', 2, new Peao(tab, Cor.Branca));
            colocarNovaPeca('h', 2, new Peao(tab, Cor.Branca));

            colocarNovaPeca('a', 8, new Torre(tab, Cor.Preta));
            colocarNovaPeca('b', 8, new Cavalo(tab, Cor.Preta));
            colocarNovaPeca('c', 8, new Bispo(tab, Cor.Preta));
            colocarNovaPeca('d', 8, new Dama(tab, Cor.Preta));
            colocarNovaPeca('e', 8, new Rei(tab, Cor.Preta, this));
            colocarNovaPeca('f', 8, new Bispo(tab, Cor.Preta));
            colocarNovaPeca('g', 8, new Cavalo(tab, Cor.Preta));
            colocarNovaPeca('h', 8, new Torre(tab, Cor.Preta));
            
            colocarNovaPeca('a', 7, new Peao(tab, Cor.Preta));
            colocarNovaPeca('b', 7, new Peao(tab, Cor.Preta));
            colocarNovaPeca('c', 7, new Peao(tab, Cor.Preta));
            colocarNovaPeca('d', 7, new Peao(tab, Cor.Preta));
            colocarNovaPeca('e', 7, new Peao(tab, Cor.Preta));
            colocarNovaPeca('f', 7, new Peao(tab, Cor.Preta));
            colocarNovaPeca('g', 7, new Peao(tab, Cor.Preta));
            colocarNovaPeca('h', 7, new Peao(tab, Cor.Preta));
        }
    }
}