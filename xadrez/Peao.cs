using tabuleiro;

namespace xadrez {
    class Peao : Peca {
        public Peao(Tabuleiro tab, Cor cor) : base (tab, cor){}

        public override string ToString() {
            return "P";
        } 

        private bool livre (Posicao pos) {
            Peca p = tab.peca(pos);
            return p == null;
        }

        private bool pecaInimiga(Posicao pos) {
            Peca p = tab.peca(pos);
            return p != null && p.cor != this.cor;
        }
        public override bool[,] movimentosPossiveis() {
            bool[,] mat = new bool[tab.linhas, tab.colunas];
            Posicao pos = new Posicao(0, 0);
            if (cor == Cor.Branca) {
                //acima
                pos.definirValores(posicao.linha-1, posicao.coluna);
                if(tab.posicaoValida(pos) && livre(pos)) {
                    mat[pos.linha, pos.coluna] = true;
                }
                //acima duplo
                pos.definirValores(posicao.linha-2, posicao.coluna);
                if(tab.posicaoValida(pos) && livre(pos) && qtdMovimentos == 0) {
                    mat[pos.linha, pos.coluna] = true;
                }
                //acima direita
                pos.definirValores(posicao.linha-1, posicao.coluna+1);
                if(tab.posicaoValida(pos) && pecaInimiga(pos)) {
                    mat[pos.linha, pos.coluna] = true;
                }            
                //cima esquerda
                pos.definirValores(posicao.linha-1, posicao.coluna-1);
                if(tab.posicaoValida(pos) && pecaInimiga(pos)) {
                    mat[pos.linha, pos.coluna] = true;
                }
            }
            else {
                //abaixo
                pos.definirValores(posicao.linha+1, posicao.coluna);
                if(tab.posicaoValida(pos) && livre(pos)) {
                    mat[pos.linha, pos.coluna] = true;
                }
                //abaixo duplo
                pos.definirValores(posicao.linha+2, posicao.coluna);
                if(tab.posicaoValida(pos) && livre(pos) && qtdMovimentos == 0) {
                    mat[pos.linha, pos.coluna] = true;
                }
                //abaixo direita
                pos.definirValores(posicao.linha+1, posicao.coluna+1);
                if(tab.posicaoValida(pos) && pecaInimiga(pos)) {
                    mat[pos.linha, pos.coluna] = true;
                }
                //abaixo esquerda
                pos.definirValores(posicao.linha+1, posicao.coluna-1);
                if(tab.posicaoValida(pos) && pecaInimiga(pos)) {
                    mat[pos.linha, pos.coluna] = true;
                }

            }
            return mat;
        }
    }
}