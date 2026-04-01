using System;
using System.Drawing;
using System.Windows.Forms;

namespace TicTacToe
{
    public partial class Form1 : Form
    {
        private Button[,] buttons;
        private char[,] board = new char[3, 3];
        private char currentPlayer = 'X';

        private Label labelTurn;
        private Button buttonReset;
        private Button buttonMenu;

        private Panel menuPanel;
        private Button btnNewGame;
        private Button btnExit;
        private Label footerMenu;

        private Panel gamePanel;
        private Label footerGame;

        private const int ButtonSize = 90;
        private const int Margin = 10;

        public Form1()
        {
            InitializeComponent();
            SetupForm();
            CreateMenu();
            CreateGameElements();
            InitializeGame();
            ShowMenu();

            this.Resize += Form1_Resize;
        }

        private void SetupForm()
        {
            this.Text = "Крестики-нолики";
            this.Size = new Size(500, 650);
            this.MinimumSize = new Size(450, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(240, 240, 240);
            this.Font = new Font("Segoe UI", 10);
        }

        private void CreateMenu()
        {
            menuPanel = new Panel
            {
                Size = new Size(300, 150),
                Location = new Point((this.ClientSize.Width - 300) / 2,
                                     (this.ClientSize.Height - 150) / 2),
                BackColor = Color.WhiteSmoke,
                BorderStyle = BorderStyle.FixedSingle,
                Anchor = AnchorStyles.None
            };

            Label title = new Label
            {
                Text = "Крестики-нолики",
                Location = new Point(0, 20),
                Size = new Size(300, 40),
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.DarkSlateBlue
            };
            menuPanel.Controls.Add(title);

            btnNewGame = new Button
            {
                Text = "Новая игра",
                Location = new Point(90, 70),
                Size = new Size(120, 35),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BackColor = Color.FromArgb(100, 149, 237),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnNewGame.Click += (s, e) => StartNewGame();
            menuPanel.Controls.Add(btnNewGame);

            btnExit = new Button
            {
                Text = "Выход",
                Location = new Point(90, 110),
                Size = new Size(120, 35),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BackColor = Color.LightGray,
                FlatStyle = FlatStyle.Flat
            };
            btnExit.Click += (s, e) => Application.Exit();
            menuPanel.Controls.Add(btnExit);

            // Подпись внизу меню
            footerMenu = new Label
            {
                Text = "Кузаков Денис Николаевич",
                AutoSize = true,
                Font = new Font("Segoe UI", 9, FontStyle.Italic),
                ForeColor = Color.Gray
            };
            menuPanel.Controls.Add(footerMenu);

            this.Controls.Add(menuPanel);
        }

        private void CreateGameElements()
        {
            gamePanel = new Panel
            {
                Dock = DockStyle.Fill,
                Visible = false
            };

            // Создаём кнопки поля (пока без позиций)
            buttons = new Button[3, 3];
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    Button btn = new Button
                    {
                        Size = new Size(ButtonSize, ButtonSize),
                        Font = new Font("Segoe UI", 28, FontStyle.Bold),
                        BackColor = Color.White,
                        FlatStyle = FlatStyle.Flat,
                        FlatAppearance = { BorderColor = Color.LightGray, BorderSize = 1 },
                        Tag = row * 10 + col
                    };
                    btn.Click += ButtonClick;
                    gamePanel.Controls.Add(btn);
                    buttons[row, col] = btn;
                }
            }

            // Метка хода
            labelTurn = new Label
            {
                AutoSize = false,
                Size = new Size(200, 50),
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                BackColor = Color.FromArgb(220, 220, 220),
                ForeColor = Color.DarkSlateGray,
                Text = "Ход: X"
            };
            gamePanel.Controls.Add(labelTurn);

            // Кнопка "Сбросить игру"
            buttonReset = new Button
            {
                Text = "Сбросить игру",
                Size = new Size(120, 40),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BackColor = Color.FromArgb(100, 149, 237),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            buttonReset.Click += (s, e) => InitializeGame();
            gamePanel.Controls.Add(buttonReset);

            // Кнопка "В меню"
            buttonMenu = new Button
            {
                Text = "В меню",
                Size = new Size(120, 40),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BackColor = Color.LightGray,
                FlatStyle = FlatStyle.Flat
            };
            buttonMenu.Click += (s, e) => ShowMenu();
            gamePanel.Controls.Add(buttonMenu);

            // Заголовок
            Label title = new Label
            {
                Text = "Крестики-нолики",
                Size = new Size(this.ClientSize.Width, 40),
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.DarkSlateBlue
            };
            gamePanel.Controls.Add(title);

            // Подпись внизу игровой панели
            footerGame = new Label
            {
                Text = "Кузаков Денис Николаевич",
                AutoSize = true,
                Font = new Font("Segoe UI", 9, FontStyle.Italic),
                ForeColor = Color.Gray
            };
            gamePanel.Controls.Add(footerGame);

            this.Controls.Add(gamePanel);

            // Первоначальная расстановка
            UpdateLayout();
        }

        private void UpdateLayout()
        {
            if (gamePanel == null || !gamePanel.Visible) return;

            int clientWidth = this.ClientSize.Width;
            int clientHeight = this.ClientSize.Height;

            // Центрирование поля
            int totalWidth = 3 * ButtonSize + 2 * Margin;
            int startX = (clientWidth - totalWidth) / 2;
            int startY = 80;

            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    buttons[row, col].Location = new Point(
                        startX + col * (ButtonSize + Margin),
                        startY + row * (ButtonSize + Margin)
                    );
                }
            }

            // Позиционирование метки хода (под полем)
            labelTurn.Location = new Point(
                (clientWidth - labelTurn.Width) / 2,
                startY + 3 * ButtonSize + 2 * Margin + 20
            );

            // Кнопки под меткой
            int buttonY = labelTurn.Location.Y + labelTurn.Height + 15;
            buttonReset.Location = new Point(
                (clientWidth - buttonReset.Width) / 2,
                buttonY
            );
            buttonMenu.Location = new Point(
                (clientWidth - buttonMenu.Width) / 2,
                buttonY + buttonReset.Height + 10
            );

            // Заголовок
            foreach (Control c in gamePanel.Controls)
            {
                if (c is Label lbl && lbl.Text == "Крестики-нолики" && lbl != labelTurn)
                {
                    lbl.Size = new Size(clientWidth, 40);
                    lbl.Location = new Point(0, 20);
                    break;
                }
            }

            // Подпись внизу
            if (footerGame != null)
            {
                footerGame.Location = new Point(
                    (clientWidth - footerGame.Width) / 2,
                    clientHeight - 25
                );
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            UpdateLayout();

            // Перецентрировать меню, если оно открыто
            if (menuPanel.Visible)
            {
                menuPanel.Location = new Point(
                    (this.ClientSize.Width - menuPanel.Width) / 2,
                    (this.ClientSize.Height - menuPanel.Height) / 2
                );
                // Подпись в меню
                if (footerMenu != null)
                {
                    footerMenu.Location = new Point(
                        (this.ClientSize.Width - footerMenu.Width) / 2,
                        this.ClientSize.Height - 25
                    );
                }
            }
        }

        private void ShowMenu()
        {
            gamePanel.Visible = false;
            menuPanel.Visible = true;
            menuPanel.BringToFront();
            // Перецентрировать меню при показе
            menuPanel.Location = new Point(
                (this.ClientSize.Width - menuPanel.Width) / 2,
                (this.ClientSize.Height - menuPanel.Height) / 2
            );
            if (footerMenu != null)
            {
                footerMenu.Location = new Point(
                    (this.ClientSize.Width - footerMenu.Width) / 2,
                    this.ClientSize.Height - 25
                );
            }
        }

        private void StartNewGame()
        {
            menuPanel.Visible = false;
            gamePanel.Visible = true;
            UpdateLayout();
            InitializeGame();
        }

        private void InitializeGame()
        {
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {
                    board[i, j] = ' ';
                    buttons[i, j].Text = "";
                    buttons[i, j].BackColor = Color.White;
                }
            currentPlayer = 'X';
            labelTurn.Text = "Ход: X";
        }

        private void ButtonClick(object sender, EventArgs e)
        {
            Button clicked = sender as Button;
            if (clicked.Text != "") return;

            int tag = (int)clicked.Tag;
            int row = tag / 10;
            int col = tag % 10;

            clicked.Text = currentPlayer.ToString();
            clicked.BackColor = (currentPlayer == 'X') ? Color.LightCyan : Color.LightYellow;
            board[row, col] = currentPlayer;

            if (CheckWin())
            {
                MessageBox.Show($"Игрок {currentPlayer} победил!", "Игра окончена");
                InitializeGame();
                return;
            }

            if (IsDraw())
            {
                MessageBox.Show("Ничья!", "Игра окончена");
                InitializeGame();
                return;
            }

            currentPlayer = (currentPlayer == 'X') ? 'O' : 'X';
            labelTurn.Text = $"Ход: {currentPlayer}";
        }

        private bool CheckWin()
        {
            for (int i = 0; i < 3; i++)
                if (board[i, 0] != ' ' && board[i, 0] == board[i, 1] && board[i, 1] == board[i, 2])
                    return true;
            for (int j = 0; j < 3; j++)
                if (board[0, j] != ' ' && board[0, j] == board[1, j] && board[1, j] == board[2, j])
                    return true;
            if (board[0, 0] != ' ' && board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2])
                return true;
            if (board[0, 2] != ' ' && board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0])
                return true;
            return false;
        }

        private bool IsDraw()
        {
            foreach (char c in board)
                if (c == ' ') return false;
            return true;
        }
    }
}