using System;

namespace Operation_Cronos {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args) {
            using (OperationCronos game = new OperationCronos()) {
                game.Run();
            }
        }
    }
}

