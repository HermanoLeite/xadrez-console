using tabuleiro;

namespace xadrez {
    class Bispo : Peca {
        public Bispo(Tabuleiro tab, Cor cor) : base (tab, cor){}

        public override string ToString() {
            return "B";
        } 

        private bool podeMover (Posicao pos) {
            Peca p = tab.peca(pos);
            return p == null || p.cor != this.cor;
        }
        public override bool[,] movimentosPossiveis() {
            bool[,] mat = new bool[tab.linhas, tab.colunas];
            //acima direita
            Posicao pos = new Posicao(posicao.linha-1, posicao.coluna+1);
            while(tab.posicaoValida(pos) && podeMover(pos)) {
                mat[pos.linha, pos.coluna] = true;
                if(tab.peca(pos)!=null) {
                    break;
                }
                pos.coluna = pos.coluna+1;
                pos.linha = pos.linha-1;
            }
            //acima esquerda
            pos.definirValores(posicao.linha-1, posicao.coluna-1);
            while(tab.posicaoValida(pos) && podeMover(pos)) {
                mat[pos.linha, pos.coluna] = true;
                if(tab.peca(pos)!=null) {
                    break;
                }
                pos.coluna = pos.coluna-1;
                pos.linha = pos.linha-1;
            }
            //abaixo direita
            pos.definirValores(posicao.linha+1, posicao.coluna+1);
            while(tab.posicaoValida(pos) && podeMover(pos)) {
                mat[pos.linha, pos.coluna] = true;
                if(tab.peca(pos)!=null) {
                    break;
                }
                pos.coluna = pos.coluna+1;
                pos.linha = pos.linha+1;
            }
            
            // abaixo esquerda
            pos.definirValores(posicao.linha+1, posicao.coluna-1);
            while(tab.posicaoValida(pos) && podeMover(pos)) {
                mat[pos.linha, pos.coluna] = true;
                if(tab.peca(pos)!=null) {
                    break;
                }
                pos.coluna = pos.coluna-1;
                pos.linha = pos.linha+1;
            }
            return mat;
        }
    }
}