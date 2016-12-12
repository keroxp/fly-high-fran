namespace FlyHighFran {
    public class GameStateChanged {
        public readonly GameState prevState;
        public readonly GameState nextState;
        public GameStateChanged(GameState prev, GameState next) {
            prevState = prev;
            nextState = next;
        }
}
}