using tabuleiro;

namespace xadrez {
    class Cavalo : Peca {
        public Cavalo(Tabuleiro tab, Cor cor) : base (tab, cor){}

        public override string ToString() {
            return "C";
        } 

        private bool podeMover (Posicao pos) {
            Peca p = tab.peca(pos);
            return p == null || p.cor != this.cor;
        }
        public override bool[,] movimentosPossiveis() {
            bool[,] mat = new bool[tab.linhas, tab.colunas];
            //1 cima, 2 esquerda
            Posicao pos = new Posicao(posicao.linha-1, posicao.coluna-2);
            if(tab.posicaoValida(pos) && podeMover(pos)) {
                mat[pos.linha, pos.coluna] = true;
            }
            //1 cima, 2 direita
            pos.definirValores(posicao.linha-1, posicao.coluna+2);
            if(tab.posicaoValida(pos) && podeMover(pos)) {
                mat[pos.linha, pos.coluna] = true;
            }
            //2 cima, 1 esquerda
            pos.definirValores(posicao.linha-2, posicao.coluna-1);
            if(tab.posicaoValida(pos) && podeMover(pos)) {
                mat[pos.linha, pos.coluna] = true;
            }
            //2 cima, 1 direita
            pos.definirValores(posicao.linha-2, posicao.coluna+1);
            if(tab.posicaoValida(pos) && podeMover(pos)) {
                mat[pos.linha, pos.coluna] = true;
            }
            //1 baixo, 2 direita
            pos.definirValores(posicao.linha+1, posicao.coluna+2);
            if(tab.posicaoValida(pos) && podeMover(pos)) {
                mat[pos.linha, pos.coluna] = true;
            }
            //1 baixo, 2 esquerda
            pos.definirValores(posicao.linha+1, posicao.coluna-2);
            if(tab.posicaoValida(pos) && podeMover(pos)) {
                mat[pos.linha, pos.coluna] = true;
            }
            //2 baixo, 1 esquerda
            pos.definirValores(posicao.linha+2, posicao.coluna-1);
            if(tab.posicaoValida(pos) && podeMover(pos)) {
                mat[pos.linha, pos.coluna] = true;
            }
            //2 baixo, 1 direita
            pos.definirValores(posicao.linha+2, posicao.coluna+1);
            if(tab.posicaoValida(pos) && podeMover(pos)) {
                mat[pos.linha, pos.coluna] = true;
            }
            return mat;
        }
    }
}