USE [ThriftShop]
GO
/****** Object:  Table [dbo].[Client]    Script Date: 11.11.2025 12:57:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Client](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[lname] [nvarchar](max) NULL,
	[fname] [nvarchar](max) NULL,
	[patronymic] [nvarchar](max) NULL,
	[phone] [nvarchar](12) NULL,
	[passportData] [nvarchar](10) NULL,
	[bankDetail] [nvarchar](16) NULL,
	[email] [nvarchar](100) NULL,
	[roleId] [int] NOT NULL,
 CONSTRAINT [PK_Client] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PaymentMethod]    Script Date: 11.11.2025 12:57:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PaymentMethod](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](max) NULL,
 CONSTRAINT [PK_PaymentMethod] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Product]    Script Date: 11.11.2025 12:57:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](max) NULL,
	[categoryId] [int] NULL,
	[description] [nvarchar](max) NULL,
	[price] [decimal](18, 0) NULL,
	[shopShare] [decimal](5, 2) NULL,
	[clientId] [int] NULL,
	[receiptDate] [date] NULL,
	[storagePeriod] [int] NULL,
	[statusId] [int] NULL,
 CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductStatus]    Script Date: 11.11.2025 12:57:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductStatus](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](max) NULL,
 CONSTRAINT [PK_ProductStatus] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductType]    Script Date: 11.11.2025 12:57:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductType](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](max) NULL,
 CONSTRAINT [PK_ProductType] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Return]    Script Date: 11.11.2025 12:57:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Return](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[productId] [int] NULL,
	[returnDate] [date] NULL,
	[reason] [nvarchar](max) NULL,
	[returnTypeId] [int] NULL,
 CONSTRAINT [PK_Return] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ReturnType]    Script Date: 11.11.2025 12:57:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReturnType](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](max) NULL,
 CONSTRAINT [PK_ReturnType] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Role]    Script Date: 11.11.2025 12:57:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](max) NULL,
 CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sales]    Script Date: 11.11.2025 12:57:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sales](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[productId] [int] NULL,
	[clientId] [int] NULL,
	[saleDate] [date] NULL,
	[payMethodId] [int] NULL,
	[totalAmount] [decimal](18, 2) NULL,
	[shopAmount] [decimal](18, 2) NULL,
	[clientAmount] [decimal](18, 2) NULL,
 CONSTRAINT [PK_Sales] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 11.11.2025 12:57:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[lname] [nvarchar](max) NULL,
	[fname] [nvarchar](max) NULL,
	[patronymic] [nvarchar](max) NULL,
	[dateBirth] [date] NULL,
	[phone] [nvarchar](12) NULL,
	[email] [nvarchar](100) NULL,
	[login] [nvarchar](100) NULL,
	[password] [nvarchar](100) NULL,
	[roleId] [int] NOT NULL,
	[isActive] [bit] NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Client] ON 

INSERT [dbo].[Client] ([id], [lname], [fname], [patronymic], [phone], [passportData], [bankDetail], [email], [roleId]) VALUES (3, N'Митюхин', N'Владимир', N'Антонович', N'79393834821', N'9111932416', N'1234432112344321', N'mituhin@gmail.com', 3)
INSERT [dbo].[Client] ([id], [lname], [fname], [patronymic], [phone], [passportData], [bankDetail], [email], [roleId]) VALUES (4, N'Иванов', N'Иван', N'Иванович', N'79123456789', N'1234567890', N'1234567890123456', N'ivanov@example.com', 3)
INSERT [dbo].[Client] ([id], [lname], [fname], [patronymic], [phone], [passportData], [bankDetail], [email], [roleId]) VALUES (5, N'Петров', N'Петр', N'Петрович', N'79234567890', N'2345678901', N'2345678901234567', N'petrov@example.com', 3)
INSERT [dbo].[Client] ([id], [lname], [fname], [patronymic], [phone], [passportData], [bankDetail], [email], [roleId]) VALUES (6, N'Сидоров', N'Сидор', N'Сидорович', N'79345678901', N'3456789012', N'3456789012345678', N'sidorov@example.com', 3)
INSERT [dbo].[Client] ([id], [lname], [fname], [patronymic], [phone], [passportData], [bankDetail], [email], [roleId]) VALUES (7, N'Кузнецов', N'Алексей', N'Алексеевич', N'79456789012', N'4567890123', N'4567890123456789', N'kuznetsov@example.com', 3)
INSERT [dbo].[Client] ([id], [lname], [fname], [patronymic], [phone], [passportData], [bankDetail], [email], [roleId]) VALUES (8, N'Михайлов', N'Михаил', N'Михайлович', N'79567890123', N'5678901234', N'5678901234567890', N'mikhailov@example.com', 3)
INSERT [dbo].[Client] ([id], [lname], [fname], [patronymic], [phone], [passportData], [bankDetail], [email], [roleId]) VALUES (9, N'Федоров', N'Федор', N'Федорович', N'79678901234', N'6789012345', N'6789012345678901', N'fedorov@example.com', 3)
INSERT [dbo].[Client] ([id], [lname], [fname], [patronymic], [phone], [passportData], [bankDetail], [email], [roleId]) VALUES (10, N'Александров', N'Александр', N'Александрович', N'79789012345', N'7890123456', N'7890123456789012', N'alexandrov@example.com', 3)
INSERT [dbo].[Client] ([id], [lname], [fname], [patronymic], [phone], [passportData], [bankDetail], [email], [roleId]) VALUES (11, N'Николаев', N'Николай', N'Николаевич', N'79890123456', N'8901234567', N'8901234567890123', N'nikolaev@example.com', 3)
INSERT [dbo].[Client] ([id], [lname], [fname], [patronymic], [phone], [passportData], [bankDetail], [email], [roleId]) VALUES (12, N'Васильев', N'Василий', N'Васильевич', N'79901234567', N'9012345678', N'9012345678901234', N'vasilev@example.com', 3)
INSERT [dbo].[Client] ([id], [lname], [fname], [patronymic], [phone], [passportData], [bankDetail], [email], [roleId]) VALUES (13, N'Григорьев', N'Григорий', N'Григорьевич', N'79012345678', N'0123456789', N'0123456789012345', N'grigoriev@example.com', 3)
INSERT [dbo].[Client] ([id], [lname], [fname], [patronymic], [phone], [passportData], [bankDetail], [email], [roleId]) VALUES (14, N'Смирнов', N'Андрей', N'Андреевич', N'79123456780', N'1234567891', N'1234567891234567', N'smirnov@example.com', 3)
INSERT [dbo].[Client] ([id], [lname], [fname], [patronymic], [phone], [passportData], [bankDetail], [email], [roleId]) VALUES (15, N'Козлов', N'Дмитрий', N'Дмитриевич', N'79234567801', N'2345678912', N'2345678912345678', N'kozlov@example.com', 3)
INSERT [dbo].[Client] ([id], [lname], [fname], [patronymic], [phone], [passportData], [bankDetail], [email], [roleId]) VALUES (16, N'Морозов', N'Сергей', N'Сергеевич', N'79345678012', N'3456789123', N'3456789123456789', N'morozov@example.com', 3)
INSERT [dbo].[Client] ([id], [lname], [fname], [patronymic], [phone], [passportData], [bankDetail], [email], [roleId]) VALUES (17, N'Егоров', N'Егор', N'Егорович', N'79456780123', N'4567891234', N'4567891234567890', N'egorov@example.com', 3)
INSERT [dbo].[Client] ([id], [lname], [fname], [patronymic], [phone], [passportData], [bankDetail], [email], [roleId]) VALUES (18, N'Павлов', N'Павел', N'Павлович', N'79567801234', N'5678912345', N'5678912345678901', N'pavlov@example.com', 3)
INSERT [dbo].[Client] ([id], [lname], [fname], [patronymic], [phone], [passportData], [bankDetail], [email], [roleId]) VALUES (19, N'Семенов', N'Семен', N'Семенович', N'79678012345', N'6789123456', N'6789123456789012', N'semenov@example.com', 3)
INSERT [dbo].[Client] ([id], [lname], [fname], [patronymic], [phone], [passportData], [bankDetail], [email], [roleId]) VALUES (20, N'Тимофеев', N'Тимофей', N'Тимофеевич', N'79780123456', N'7891234567', N'7891234567890123', N'timofeev@example.com', 3)
INSERT [dbo].[Client] ([id], [lname], [fname], [patronymic], [phone], [passportData], [bankDetail], [email], [roleId]) VALUES (21, N'Ковалев', N'Владимир', N'Владимирович', N'79890123457', N'8901234568', N'8901234568901234', N'kovalev@example.com', 3)
INSERT [dbo].[Client] ([id], [lname], [fname], [patronymic], [phone], [passportData], [bankDetail], [email], [roleId]) VALUES (22, N'Лебедев', N'Игорь', N'Игоревич', N'79901234578', N'9012345679', N'9012345679012345', N'lebedev@example.com', 3)
INSERT [dbo].[Client] ([id], [lname], [fname], [patronymic], [phone], [passportData], [bankDetail], [email], [roleId]) VALUES (23, N'Зайцев', N'Роман', N'Романович', N'79012345789', N'0123456780', N'0123456780123456', N'zaitsev@example.com', 3)
INSERT [dbo].[Client] ([id], [lname], [fname], [patronymic], [phone], [passportData], [bankDetail], [email], [roleId]) VALUES (24, N'Белов', N'Артем', N'Артемович', N'79123457890', N'1234567892', N'1234567892345678', N'belov@example.com', 3)
INSERT [dbo].[Client] ([id], [lname], [fname], [patronymic], [phone], [passportData], [bankDetail], [email], [roleId]) VALUES (25, N'Чернов', N'Максим', N'Максимович', N'79234578901', N'2345678913', N'2345678913456789', N'chernov@example.com', 3)
INSERT [dbo].[Client] ([id], [lname], [fname], [patronymic], [phone], [passportData], [bankDetail], [email], [roleId]) VALUES (26, N'Шестаков', N'Даниил', N'Даниилович', N'79345789012', N'3456789124', N'3456789124567890', N'shestakov@example.com', 3)
INSERT [dbo].[Client] ([id], [lname], [fname], [patronymic], [phone], [passportData], [bankDetail], [email], [roleId]) VALUES (27, N'Кузьмин', N'Кирилл', N'Кириллович', N'79457890123', N'4567891235', N'4567891235678901', N'kuzmin@example.com', 3)
INSERT [dbo].[Client] ([id], [lname], [fname], [patronymic], [phone], [passportData], [bankDetail], [email], [roleId]) VALUES (28, N'Макаров', N'Степан', N'Степанович', N'79578901234', N'5678912346', N'5678912346789012', N'makarov@example.com', 3)
INSERT [dbo].[Client] ([id], [lname], [fname], [patronymic], [phone], [passportData], [bankDetail], [email], [roleId]) VALUES (29, N'Андреев', N'Виктор', N'Викторович', N'79789012346', N'6789123457', N'6789123457890123', N'andreev@example.com', 3)
INSERT [dbo].[Client] ([id], [lname], [fname], [patronymic], [phone], [passportData], [bankDetail], [email], [roleId]) VALUES (30, N'Никитин', N'Никита', N'Никитич', N'79890123467', N'7891234568', N'7891234568901234', N'nikitin@example.com', 3)
INSERT [dbo].[Client] ([id], [lname], [fname], [patronymic], [phone], [passportData], [bankDetail], [email], [roleId]) VALUES (31, N'Романов', N'Ярослав', N'Ярославич', N'79901234678', N'8901234569', N'8901234569012345', N'romanov@example.com', 3)
INSERT [dbo].[Client] ([id], [lname], [fname], [patronymic], [phone], [passportData], [bankDetail], [email], [roleId]) VALUES (32, N'Захаров', N'Матвей', N'Матвеевич', N'79012346789', N'9012345670', N'9012345670123456', N'zaharov@example.com', 3)
INSERT [dbo].[Client] ([id], [lname], [fname], [patronymic], [phone], [passportData], [bankDetail], [email], [roleId]) VALUES (33, N'Борисов', N'Платон', N'Платонович', N'79123467890', N'0123456781', N'0123456781234567', N'borisov@example.com', 3)
INSERT [dbo].[Client] ([id], [lname], [fname], [patronymic], [phone], [passportData], [bankDetail], [email], [roleId]) VALUES (34, N'Гусев', N'Лев', N'Львович', N'79234678901', N'1234567893', N'1234567893456789', N'gusev@example.com', 3)
INSERT [dbo].[Client] ([id], [lname], [fname], [patronymic], [phone], [passportData], [bankDetail], [email], [roleId]) VALUES (35, N'Медведев', N'Марк', N'Маркович', N'79346789012', N'2345678914', N'2345678914567890', N'medvedev@example.com', 3)
INSERT [dbo].[Client] ([id], [lname], [fname], [patronymic], [phone], [passportData], [bankDetail], [email], [roleId]) VALUES (36, N'Тихонов', N'Илья', N'Ильич', N'79467890123', N'3456789125', N'3456789125678901', N'tikhonov@example.com', 3)
INSERT [dbo].[Client] ([id], [lname], [fname], [patronymic], [phone], [passportData], [bankDetail], [email], [roleId]) VALUES (37, N'Кудрявцев', N'Денис', N'Денисович', N'79578901235', N'4567891236', N'4567891236789012', N'kudryavtsev@example.com', 3)
INSERT [dbo].[Client] ([id], [lname], [fname], [patronymic], [phone], [passportData], [bankDetail], [email], [roleId]) VALUES (38, N'Беляков', N'Глеб', N'Глебович', N'79678901236', N'5678912347', N'5678912347890123', N'belyakov@example.com', 3)
INSERT [dbo].[Client] ([id], [lname], [fname], [patronymic], [phone], [passportData], [bankDetail], [email], [roleId]) VALUES (39, N'Воробьев', N'Арсений', N'Арсеньевич', N'79789012356', N'6789123458', N'6789123458901234', N'vorobiev@example.com', 3)
INSERT [dbo].[Client] ([id], [lname], [fname], [patronymic], [phone], [passportData], [bankDetail], [email], [roleId]) VALUES (40, N'Соколов', N'Тимур', N'Тимурович', N'79890123567', N'7891234569', N'7891234569012345', N'sokolov@example.com', 3)
INSERT [dbo].[Client] ([id], [lname], [fname], [patronymic], [phone], [passportData], [bankDetail], [email], [roleId]) VALUES (41, N'Миронов', N'Родион', N'Родионович', N'79901235678', N'8901234560', N'8901234560123456', N'mironov@example.com', 3)
SET IDENTITY_INSERT [dbo].[Client] OFF
GO
SET IDENTITY_INSERT [dbo].[PaymentMethod] ON 

INSERT [dbo].[PaymentMethod] ([id], [name]) VALUES (1, N'Наличные')
INSERT [dbo].[PaymentMethod] ([id], [name]) VALUES (2, N'Безналичный')
SET IDENTITY_INSERT [dbo].[PaymentMethod] OFF
GO
SET IDENTITY_INSERT [dbo].[Product] ON 

INSERT [dbo].[Product] ([id], [name], [categoryId], [description], [price], [shopShare], [clientId], [receiptDate], [storagePeriod], [statusId]) VALUES (1, N'Смартфон Samsung Galaxy S21', 1, N'Новый смартфон с гарантией', CAST(30000 AS Decimal(18, 0)), CAST(10.00 AS Decimal(5, 2)), 3, CAST(N'2025-10-11' AS Date), 30, 1)
INSERT [dbo].[Product] ([id], [name], [categoryId], [description], [price], [shopShare], [clientId], [receiptDate], [storagePeriod], [statusId]) VALUES (2, N'Наушники Sony WH-1000XM4', 1, N'Беспроводные наушники с шумоподавлением', CAST(15000 AS Decimal(18, 0)), CAST(15.00 AS Decimal(5, 2)), 4, CAST(N'2024-10-01' AS Date), 30, 1)
INSERT [dbo].[Product] ([id], [name], [categoryId], [description], [price], [shopShare], [clientId], [receiptDate], [storagePeriod], [statusId]) VALUES (3, N'Планшет Apple iPad Air 4', 1, N'Планшет с поддержкой Apple Pencil', CAST(40000 AS Decimal(18, 0)), CAST(12.00 AS Decimal(5, 2)), 5, CAST(N'2025-10-01' AS Date), 30, 1)
INSERT [dbo].[Product] ([id], [name], [categoryId], [description], [price], [shopShare], [clientId], [receiptDate], [storagePeriod], [statusId]) VALUES (4, N'Фотокамера Canon EOS R5', 1, N'Профессиональная беззеркальная камера', CAST(120000 AS Decimal(18, 0)), CAST(8.00 AS Decimal(5, 2)), 6, CAST(N'2025-10-01' AS Date), 30, 2)
INSERT [dbo].[Product] ([id], [name], [categoryId], [description], [price], [shopShare], [clientId], [receiptDate], [storagePeriod], [statusId]) VALUES (5, N'Кожаная куртка Zara', 2, N'Стильная кожаная куртка черного цвета', CAST(8000 AS Decimal(18, 0)), CAST(20.00 AS Decimal(5, 2)), 7, CAST(N'2024-10-01' AS Date), 30, 1)
INSERT [dbo].[Product] ([id], [name], [categoryId], [description], [price], [shopShare], [clientId], [receiptDate], [storagePeriod], [statusId]) VALUES (6, N'Часы Rolex Submariner', 2, N'Премиальные механические часы', CAST(500000 AS Decimal(18, 0)), CAST(5.00 AS Decimal(5, 2)), 8, CAST(N'2025-10-01' AS Date), 30, 1)
INSERT [dbo].[Product] ([id], [name], [categoryId], [description], [price], [shopShare], [clientId], [receiptDate], [storagePeriod], [statusId]) VALUES (7, N'Шарф из кашемира Burberry', 2, N'Шарф ручной работы', CAST(12000 AS Decimal(18, 0)), CAST(18.00 AS Decimal(5, 2)), 9, CAST(N'2025-10-01' AS Date), 30, 1)
INSERT [dbo].[Product] ([id], [name], [categoryId], [description], [price], [shopShare], [clientId], [receiptDate], [storagePeriod], [statusId]) VALUES (8, N'Кроссовки Nike Air Max', 2, N'Кроссовки в отличном состоянии', CAST(5000 AS Decimal(18, 0)), CAST(25.00 AS Decimal(5, 2)), 10, CAST(N'2025-10-01' AS Date), 30, 2)
INSERT [dbo].[Product] ([id], [name], [categoryId], [description], [price], [shopShare], [clientId], [receiptDate], [storagePeriod], [statusId]) VALUES (9, N'Мастер и Маргарита, Булгаков', 3, N'Классическое издание романа', CAST(500 AS Decimal(18, 0)), CAST(30.00 AS Decimal(5, 2)), 11, CAST(N'2025-10-01' AS Date), 30, 1)
INSERT [dbo].[Product] ([id], [name], [categoryId], [description], [price], [shopShare], [clientId], [receiptDate], [storagePeriod], [statusId]) VALUES (10, N'1984, Джордж Оруэлл', 3, N'Антиутопия о тоталитарном обществе', CAST(400 AS Decimal(18, 0)), CAST(30.00 AS Decimal(5, 2)), 12, CAST(N'2025-10-01' AS Date), 30, 1)
INSERT [dbo].[Product] ([id], [name], [categoryId], [description], [price], [shopShare], [clientId], [receiptDate], [storagePeriod], [statusId]) VALUES (11, N'Гарри Поттер и философский камень', 3, N'Первая книга серии о Гарри Поттере', CAST(1000 AS Decimal(18, 0)), CAST(20.00 AS Decimal(5, 2)), 13, CAST(N'2025-10-01' AS Date), 30, 1)
INSERT [dbo].[Product] ([id], [name], [categoryId], [description], [price], [shopShare], [clientId], [receiptDate], [storagePeriod], [statusId]) VALUES (12, N'Сто лет одиночества, Габриэль Гарсиа Маркес', 3, N'Латиноамериканская классика', CAST(700 AS Decimal(18, 0)), CAST(25.00 AS Decimal(5, 2)), 14, CAST(N'2025-10-01' AS Date), 30, 1)
INSERT [dbo].[Product] ([id], [name], [categoryId], [description], [price], [shopShare], [clientId], [receiptDate], [storagePeriod], [statusId]) VALUES (13, N'Диван угловой IKEA', 4, N'Удобный диван для гостиной', CAST(25000 AS Decimal(18, 0)), CAST(15.00 AS Decimal(5, 2)), 15, CAST(N'2025-10-01' AS Date), 30, 1)
INSERT [dbo].[Product] ([id], [name], [categoryId], [description], [price], [shopShare], [clientId], [receiptDate], [storagePeriod], [statusId]) VALUES (14, N'Стол обеденный деревянный', 4, N'Массив дерева, современный дизайн', CAST(15000 AS Decimal(18, 0)), CAST(15.00 AS Decimal(5, 2)), 16, CAST(N'2025-10-01' AS Date), 30, 1)
INSERT [dbo].[Product] ([id], [name], [categoryId], [description], [price], [shopShare], [clientId], [receiptDate], [storagePeriod], [statusId]) VALUES (15, N'Шкаф-купе двухстворчатый', 4, N'Шкаф с зеркальными дверями', CAST(18000 AS Decimal(18, 0)), CAST(12.00 AS Decimal(5, 2)), 17, CAST(N'2024-10-01' AS Date), 30, 1)
INSERT [dbo].[Product] ([id], [name], [categoryId], [description], [price], [shopShare], [clientId], [receiptDate], [storagePeriod], [statusId]) VALUES (16, N'Кресло офисное ergonomic', 4, N'Кресло с регулировкой высоты и подлокотников', CAST(8000 AS Decimal(18, 0)), CAST(20.00 AS Decimal(5, 2)), 18, CAST(N'2025-10-01' AS Date), 30, 1)
INSERT [dbo].[Product] ([id], [name], [categoryId], [description], [price], [shopShare], [clientId], [receiptDate], [storagePeriod], [statusId]) VALUES (17, N'Велосипед горный Stels Navigator', 5, N'Велосипед для пересеченной местности', CAST(20000 AS Decimal(18, 0)), CAST(10.00 AS Decimal(5, 2)), 19, CAST(N'2024-10-01' AS Date), 30, 3)
INSERT [dbo].[Product] ([id], [name], [categoryId], [description], [price], [shopShare], [clientId], [receiptDate], [storagePeriod], [statusId]) VALUES (18, N'Лыжи Fischer RC4', 5, N'Гоночные лыжи для профессионалов', CAST(35000 AS Decimal(18, 0)), CAST(8.00 AS Decimal(5, 2)), 20, CAST(N'2025-10-01' AS Date), 30, 1)
INSERT [dbo].[Product] ([id], [name], [categoryId], [description], [price], [shopShare], [clientId], [receiptDate], [storagePeriod], [statusId]) VALUES (19, N'Тренажер беговая дорожка', 5, N'Домашний тренажер с электродвигателем', CAST(50000 AS Decimal(18, 0)), CAST(12.00 AS Decimal(5, 2)), 21, CAST(N'2025-10-01' AS Date), 30, 1)
INSERT [dbo].[Product] ([id], [name], [categoryId], [description], [price], [shopShare], [clientId], [receiptDate], [storagePeriod], [statusId]) VALUES (20, N'Рюкзак туристический Deuter', 5, N'Прочный рюкзак для длительных походов', CAST(6000 AS Decimal(18, 0)), CAST(20.00 AS Decimal(5, 2)), 22, CAST(N'2025-10-01' AS Date), 30, 1)
INSERT [dbo].[Product] ([id], [name], [categoryId], [description], [price], [shopShare], [clientId], [receiptDate], [storagePeriod], [statusId]) VALUES (21, N'Коляска прогулочная BabyCare', 6, N'Легкая и маневренная коляска', CAST(12000 AS Decimal(18, 0)), CAST(15.00 AS Decimal(5, 2)), 23, CAST(N'2025-10-01' AS Date), 30, 1)
INSERT [dbo].[Product] ([id], [name], [categoryId], [description], [price], [shopShare], [clientId], [receiptDate], [storagePeriod], [statusId]) VALUES (22, N'Игровой набор LEGO Technic', 6, N'Конструктор для детей от 8 лет', CAST(3000 AS Decimal(18, 0)), CAST(25.00 AS Decimal(5, 2)), 24, CAST(N'2025-10-01' AS Date), 30, 1)
INSERT [dbo].[Product] ([id], [name], [categoryId], [description], [price], [shopShare], [clientId], [receiptDate], [storagePeriod], [statusId]) VALUES (23, N'Манеж для новорожденных', 6, N'Безопасный манеж с матрасом', CAST(8000 AS Decimal(18, 0)), CAST(20.00 AS Decimal(5, 2)), 25, CAST(N'2025-10-01' AS Date), 30, 1)
INSERT [dbo].[Product] ([id], [name], [categoryId], [description], [price], [shopShare], [clientId], [receiptDate], [storagePeriod], [statusId]) VALUES (24, N'Детский велосипед Novatrack', 6, N'Велосипед для детей от 5 лет', CAST(9000 AS Decimal(18, 0)), CAST(18.00 AS Decimal(5, 2)), 26, CAST(N'2024-10-01' AS Date), 30, 1)
INSERT [dbo].[Product] ([id], [name], [categoryId], [description], [price], [shopShare], [clientId], [receiptDate], [storagePeriod], [statusId]) VALUES (25, N'Дрель ударная Bosch GSB 1600', 7, N'Профессиональная дрель с функцией удара', CAST(12000 AS Decimal(18, 0)), CAST(12.00 AS Decimal(5, 2)), 27, CAST(N'2025-10-01' AS Date), 30, 1)
INSERT [dbo].[Product] ([id], [name], [categoryId], [description], [price], [shopShare], [clientId], [receiptDate], [storagePeriod], [statusId]) VALUES (26, N'Перфоратор Makita HR2470', 7, N'Мощный перфоратор для строительных работ', CAST(18000 AS Decimal(18, 0)), CAST(10.00 AS Decimal(5, 2)), 28, CAST(N'2025-10-01' AS Date), 30, 1)
INSERT [dbo].[Product] ([id], [name], [categoryId], [description], [price], [shopShare], [clientId], [receiptDate], [storagePeriod], [statusId]) VALUES (27, N'Шлифовальная машинка DeWalt DWE6423', 7, N'Универсальная шлифовальная машинка', CAST(10000 AS Decimal(18, 0)), CAST(15.00 AS Decimal(5, 2)), 29, CAST(N'2025-10-01' AS Date), 30, 1)
INSERT [dbo].[Product] ([id], [name], [categoryId], [description], [price], [shopShare], [clientId], [receiptDate], [storagePeriod], [statusId]) VALUES (28, N'Циркулярная пила Metabo KS 216 M', 7, N'Пила для распиловки древесины', CAST(25000 AS Decimal(18, 0)), CAST(10.00 AS Decimal(5, 2)), 30, CAST(N'2025-10-01' AS Date), 30, 1)
INSERT [dbo].[Product] ([id], [name], [categoryId], [description], [price], [shopShare], [clientId], [receiptDate], [storagePeriod], [statusId]) VALUES (29, N'Холодильник LG GA-B459 UCA', 8, N'Двухкамерный холодильник с No Frost', CAST(40000 AS Decimal(18, 0)), CAST(10.00 AS Decimal(5, 2)), 31, CAST(N'2025-10-01' AS Date), 30, 1)
INSERT [dbo].[Product] ([id], [name], [categoryId], [description], [price], [shopShare], [clientId], [receiptDate], [storagePeriod], [statusId]) VALUES (30, N'Пылесос Dyson V15 Detect', 8, N'Безмешковый пылесос с лазерным обнаружением пыли', CAST(35000 AS Decimal(18, 0)), CAST(12.00 AS Decimal(5, 2)), 32, CAST(N'2025-10-01' AS Date), 30, 1)
INSERT [dbo].[Product] ([id], [name], [categoryId], [description], [price], [shopShare], [clientId], [receiptDate], [storagePeriod], [statusId]) VALUES (31, N'Стиральная машина Samsung WW80T554DAW', 8, N'Автоматическая стиральная машина с прямым приводом', CAST(30000 AS Decimal(18, 0)), CAST(10.00 AS Decimal(5, 2)), 33, CAST(N'2024-10-01' AS Date), 30, 3)
INSERT [dbo].[Product] ([id], [name], [categoryId], [description], [price], [shopShare], [clientId], [receiptDate], [storagePeriod], [statusId]) VALUES (32, N'Микроволновая печь Panasonic NN-ST27JW', 8, N'Микроволновка с грилем и конвекцией', CAST(10000 AS Decimal(18, 0)), CAST(15.00 AS Decimal(5, 2)), 34, CAST(N'2025-10-01' AS Date), 30, 1)
INSERT [dbo].[Product] ([id], [name], [categoryId], [description], [price], [shopShare], [clientId], [receiptDate], [storagePeriod], [statusId]) VALUES (33, N'Автомагнитола Pioneer MVH-S320BT', 9, N'Автомагнитола с Bluetooth и USB', CAST(8000 AS Decimal(18, 0)), CAST(15.00 AS Decimal(5, 2)), 35, CAST(N'2025-10-01' AS Date), 30, 1)
INSERT [dbo].[Product] ([id], [name], [categoryId], [description], [price], [shopShare], [clientId], [receiptDate], [storagePeriod], [statusId]) VALUES (34, N'Шины зимние Bridgestone Blizzak DM-V2', 9, N'Зимние шины для внедорожников', CAST(12000 AS Decimal(18, 0)), CAST(10.00 AS Decimal(5, 2)), 36, CAST(N'2025-10-01' AS Date), 30, 1)
INSERT [dbo].[Product] ([id], [name], [categoryId], [description], [price], [shopShare], [clientId], [receiptDate], [storagePeriod], [statusId]) VALUES (35, N'Автокресло детское Britax Römer', 9, N'Безопасное автокресло для детей', CAST(15000 AS Decimal(18, 0)), CAST(12.00 AS Decimal(5, 2)), 37, CAST(N'2025-10-01' AS Date), 30, 1)
INSERT [dbo].[Product] ([id], [name], [categoryId], [description], [price], [shopShare], [clientId], [receiptDate], [storagePeriod], [statusId]) VALUES (36, N'Видеорегистратор Artway AV-308', 9, N'Full HD видеорегистратор с ночным видением', CAST(5000 AS Decimal(18, 0)), CAST(20.00 AS Decimal(5, 2)), 38, CAST(N'2025-10-01' AS Date), 30, 1)
INSERT [dbo].[Product] ([id], [name], [categoryId], [description], [price], [shopShare], [clientId], [receiptDate], [storagePeriod], [statusId]) VALUES (37, N'Золотое кольцо с бриллиантами', 10, N'Кольцо из белого золота с бриллиантами', CAST(100000 AS Decimal(18, 0)), CAST(5.00 AS Decimal(5, 2)), 39, CAST(N'2025-10-01' AS Date), 30, 1)
INSERT [dbo].[Product] ([id], [name], [categoryId], [description], [price], [shopShare], [clientId], [receiptDate], [storagePeriod], [statusId]) VALUES (38, N'Серебряная цепочка Pandora', 10, N'Цепочка из серебра с плетением "венецианка"', CAST(5000 AS Decimal(18, 0)), CAST(20.00 AS Decimal(5, 2)), 40, CAST(N'2024-10-01' AS Date), 30, 1)
INSERT [dbo].[Product] ([id], [name], [categoryId], [description], [price], [shopShare], [clientId], [receiptDate], [storagePeriod], [statusId]) VALUES (39, N'Серьги с жемчугом', 10, N'Серьги с натуральным жемчугом', CAST(20000 AS Decimal(18, 0)), CAST(10.00 AS Decimal(5, 2)), 41, CAST(N'2025-10-01' AS Date), 30, 1)
INSERT [dbo].[Product] ([id], [name], [categoryId], [description], [price], [shopShare], [clientId], [receiptDate], [storagePeriod], [statusId]) VALUES (40, N'Золотой браслет с рубинами', 10, N'Браслет из красного золота с рубинами', CAST(80000 AS Decimal(18, 0)), CAST(5.00 AS Decimal(5, 2)), 3, CAST(N'2025-10-01' AS Date), 30, 1)
INSERT [dbo].[Product] ([id], [name], [categoryId], [description], [price], [shopShare], [clientId], [receiptDate], [storagePeriod], [statusId]) VALUES (42, NULL, NULL, NULL, NULL, NULL, NULL, CAST(N'2025-10-01' AS Date), NULL, NULL)
SET IDENTITY_INSERT [dbo].[Product] OFF
GO
SET IDENTITY_INSERT [dbo].[ProductStatus] ON 

INSERT [dbo].[ProductStatus] ([id], [name]) VALUES (1, N'На витрине')
INSERT [dbo].[ProductStatus] ([id], [name]) VALUES (2, N'Продан')
INSERT [dbo].[ProductStatus] ([id], [name]) VALUES (3, N'Возвращен')
INSERT [dbo].[ProductStatus] ([id], [name]) VALUES (4, N'Списание')
SET IDENTITY_INSERT [dbo].[ProductStatus] OFF
GO
SET IDENTITY_INSERT [dbo].[ProductType] ON 

INSERT [dbo].[ProductType] ([id], [name]) VALUES (1, N'Электроника')
INSERT [dbo].[ProductType] ([id], [name]) VALUES (2, N'Одежда и аксессуары')
INSERT [dbo].[ProductType] ([id], [name]) VALUES (3, N'Книги')
INSERT [dbo].[ProductType] ([id], [name]) VALUES (4, N'Мебель')
INSERT [dbo].[ProductType] ([id], [name]) VALUES (5, N'Спорт и отдых')
INSERT [dbo].[ProductType] ([id], [name]) VALUES (6, N'Детские товары')
INSERT [dbo].[ProductType] ([id], [name]) VALUES (7, N'Инструменты и оборудование')
INSERT [dbo].[ProductType] ([id], [name]) VALUES (8, N'Бытовая техника')
INSERT [dbo].[ProductType] ([id], [name]) VALUES (9, N'Автотовары')
INSERT [dbo].[ProductType] ([id], [name]) VALUES (10, N'Ювелирные изделия')
SET IDENTITY_INSERT [dbo].[ProductType] OFF
GO
SET IDENTITY_INSERT [dbo].[Return] ON 

INSERT [dbo].[Return] ([id], [productId], [returnDate], [reason], [returnTypeId]) VALUES (1, 17, CAST(N'2025-04-26' AS Date), N'ddssdfsd', 2)
INSERT [dbo].[Return] ([id], [productId], [returnDate], [reason], [returnTypeId]) VALUES (2, 31, CAST(N'2025-03-26' AS Date), N'sdad', 2)
SET IDENTITY_INSERT [dbo].[Return] OFF
GO
SET IDENTITY_INSERT [dbo].[ReturnType] ON 

INSERT [dbo].[ReturnType] ([id], [name]) VALUES (1, N'Списание')
INSERT [dbo].[ReturnType] ([id], [name]) VALUES (2, N'Возврат собственнику')
SET IDENTITY_INSERT [dbo].[ReturnType] OFF
GO
SET IDENTITY_INSERT [dbo].[Role] ON 

INSERT [dbo].[Role] ([id], [name]) VALUES (1, N'Админ')
INSERT [dbo].[Role] ([id], [name]) VALUES (2, N'Работник')
INSERT [dbo].[Role] ([id], [name]) VALUES (3, N'Клиент')
INSERT [dbo].[Role] ([id], [name]) VALUES (4, N'Кассир')
INSERT [dbo].[Role] ([id], [name]) VALUES (5, N'Менеджер по приему')
INSERT [dbo].[Role] ([id], [name]) VALUES (6, N'Бухгалтер')
SET IDENTITY_INSERT [dbo].[Role] OFF
GO
SET IDENTITY_INSERT [dbo].[Sales] ON 

INSERT [dbo].[Sales] ([id], [productId], [clientId], [saleDate], [payMethodId], [totalAmount], [shopAmount], [clientAmount]) VALUES (2, 4, 3, CAST(N'2025-04-26' AS Date), 2, CAST(120000.00 AS Decimal(18, 2)), CAST(9600.00 AS Decimal(18, 2)), CAST(110400.00 AS Decimal(18, 2)))
INSERT [dbo].[Sales] ([id], [productId], [clientId], [saleDate], [payMethodId], [totalAmount], [shopAmount], [clientAmount]) VALUES (3, 8, 12, CAST(N'2025-03-26' AS Date), 1, CAST(5000.00 AS Decimal(18, 2)), CAST(1250.00 AS Decimal(18, 2)), CAST(3750.00 AS Decimal(18, 2)))
SET IDENTITY_INSERT [dbo].[Sales] OFF
GO
SET IDENTITY_INSERT [dbo].[User] ON 

INSERT [dbo].[User] ([id], [lname], [fname], [patronymic], [dateBirth], [phone], [email], [login], [password], [roleId], [isActive]) VALUES (1, N'Иванов', N'Иван', N'Иванович', CAST(N'1985-05-15' AS Date), N'12345678901', N'ivan@example.com', N'fara', N'123', 1, 1)
INSERT [dbo].[User] ([id], [lname], [fname], [patronymic], [dateBirth], [phone], [email], [login], [password], [roleId], [isActive]) VALUES (2, N'Петров', N'Петр', N'Петрович', CAST(N'1990-06-20' AS Date), N'12345678902', N'petrov@example.com', N'fara', N'123', 4, 1)
INSERT [dbo].[User] ([id], [lname], [fname], [patronymic], [dateBirth], [phone], [email], [login], [password], [roleId], [isActive]) VALUES (3, N'Сидорова', N'Мария', N'Сидоровна', CAST(N'1987-08-10' AS Date), N'12345678903', N'sidorova@example.com', N'fara', N'123', 5, 1)
INSERT [dbo].[User] ([id], [lname], [fname], [patronymic], [dateBirth], [phone], [email], [login], [password], [roleId], [isActive]) VALUES (4, N'Кузнецов', N'Сергей', N'Кузнецович', CAST(N'1992-04-25' AS Date), N'12345678904', N'kuznetsov@example.com', N'fara', N'123', 6, 1)
INSERT [dbo].[User] ([id], [lname], [fname], [patronymic], [dateBirth], [phone], [email], [login], [password], [roleId], [isActive]) VALUES (5, N'Морозова', N'Елена', N'Морозовна', CAST(N'1980-11-30' AS Date), N'12345678905', N'morozova@example.com', N'morozova123', N'pass123', 4, 1)
INSERT [dbo].[User] ([id], [lname], [fname], [patronymic], [dateBirth], [phone], [email], [login], [password], [roleId], [isActive]) VALUES (6, N'Смирнов', N'Александр', N'Смирнович', CAST(N'1995-02-14' AS Date), N'12345678906', N'smirnov@example.com', N'smirnov123', N'pass123', 5, 1)
INSERT [dbo].[User] ([id], [lname], [fname], [patronymic], [dateBirth], [phone], [email], [login], [password], [roleId], [isActive]) VALUES (7, N'Федоров', N'Дмитрий', N'Федорович', CAST(N'1991-12-01' AS Date), N'12345678907', N'fedotov@example.com', N'fedotov123', N'pass123', 6, 1)
INSERT [dbo].[User] ([id], [lname], [fname], [patronymic], [dateBirth], [phone], [email], [login], [password], [roleId], [isActive]) VALUES (8, N'Захарова', N'Татьяна', N'Захаровна', CAST(N'1988-03-19' AS Date), N'12345678908', N'zakhareva@example.com', N'zakhareva123', N'pass123', 4, 1)
INSERT [dbo].[User] ([id], [lname], [fname], [patronymic], [dateBirth], [phone], [email], [login], [password], [roleId], [isActive]) VALUES (9, N'Воробьева', N'Анна', N'Воробьевна', CAST(N'1983-01-09' AS Date), N'12345678909', N'vorobyova@example.com', N'vorobyova123', N'pass123', 5, 1)
INSERT [dbo].[User] ([id], [lname], [fname], [patronymic], [dateBirth], [phone], [email], [login], [password], [roleId], [isActive]) VALUES (10, N'Григорьев', N'Анатолий', N'Григорьевич', CAST(N'1994-07-07' AS Date), N'12345678910', N'grigoryev@example.com', N'grigoryev123', N'pass123', 6, 1)
INSERT [dbo].[User] ([id], [lname], [fname], [patronymic], [dateBirth], [phone], [email], [login], [password], [roleId], [isActive]) VALUES (11, N'Кремлева', N'Виктория', N'Кремлевич', CAST(N'1986-09-18' AS Date), N'12345678911', N'kremlev@example.com', N'kremlev123', N'pass123', 2, 1)
INSERT [dbo].[User] ([id], [lname], [fname], [patronymic], [dateBirth], [phone], [email], [login], [password], [roleId], [isActive]) VALUES (12, N'Соловьева', N'Ольга', N'Соловьевна', CAST(N'1993-10-30' AS Date), N'12345678912', N'solovyeva@example.com', N'solovyeva123', N'pass123', 2, 1)
INSERT [dbo].[User] ([id], [lname], [fname], [patronymic], [dateBirth], [phone], [email], [login], [password], [roleId], [isActive]) VALUES (13, N'Лебедев', N'Николай', N'Лебедевич', CAST(N'1982-05-22' AS Date), N'12345678913', N'lebedyev@example.com', N'lebedyev123', N'pass123', 2, 1)
INSERT [dbo].[User] ([id], [lname], [fname], [patronymic], [dateBirth], [phone], [email], [login], [password], [roleId], [isActive]) VALUES (14, N'Беляева', N'Варя', N'Беляевна', CAST(N'1989-06-15' AS Date), N'12345678914', N'belyaeva@example.com', N'belyaeva123', N'pass123', 2, 1)
INSERT [dbo].[User] ([id], [lname], [fname], [patronymic], [dateBirth], [phone], [email], [login], [password], [roleId], [isActive]) VALUES (15, N'Дмитриева', N'Ксения', N'Дмитриевна', CAST(N'1996-03-25' AS Date), N'12345678915', N'dmitrieva@example.com', N'dmitrieva123', N'pass123', 2, 1)
INSERT [dbo].[User] ([id], [lname], [fname], [patronymic], [dateBirth], [phone], [email], [login], [password], [roleId], [isActive]) VALUES (16, N'Коваленко', N'Светлана', N'Коваленко', CAST(N'1991-07-11' AS Date), N'12345678916', N'kovalenko@example.com', N'kovalenko123', N'pass123', 2, 1)
INSERT [dbo].[User] ([id], [lname], [fname], [patronymic], [dateBirth], [phone], [email], [login], [password], [roleId], [isActive]) VALUES (17, N'Иванова', N'Анна', N'Петровна', CAST(N'1985-07-15' AS Date), N'12345678917', N'anna.ivanova@example.com', N'annai', N'pass123', 2, 1)
INSERT [dbo].[User] ([id], [lname], [fname], [patronymic], [dateBirth], [phone], [email], [login], [password], [roleId], [isActive]) VALUES (18, N'Смирнова', N'Елена', N'Владимировна', CAST(N'1990-08-20' AS Date), N'12345678918', N'elena.smirnova@example.com', N'elenas', N'pass123', 2, 1)
INSERT [dbo].[User] ([id], [lname], [fname], [patronymic], [dateBirth], [phone], [email], [login], [password], [roleId], [isActive]) VALUES (19, N'Кузнецова', N'Мария', N'Александровна', CAST(N'1992-09-10' AS Date), N'12345678919', N'maria.kuznetsova@example.com', N'mariak', N'pass123', 2, 1)
INSERT [dbo].[User] ([id], [lname], [fname], [patronymic], [dateBirth], [phone], [email], [login], [password], [roleId], [isActive]) VALUES (20, N'Захарова', N'Ольга', N'Сергеевна', CAST(N'1988-10-15' AS Date), N'12345678920', N'olga.zaharova@example.com', N'olgaz', N'pass123', 2, 1)
INSERT [dbo].[User] ([id], [lname], [fname], [patronymic], [dateBirth], [phone], [email], [login], [password], [roleId], [isActive]) VALUES (21, N'Григорьева', N'Татьяна', N'Дмитриевна', CAST(N'1995-11-20' AS Date), N'12345678921', N'tatyana.gribov@example.com', N'tatyang', N'pass123', 2, 1)
INSERT [dbo].[User] ([id], [lname], [fname], [patronymic], [dateBirth], [phone], [email], [login], [password], [roleId], [isActive]) VALUES (22, N'Кремлев', N'Николай', N'Иванович', CAST(N'1980-12-05' AS Date), N'12345678922', N'nikolay.kremlev@example.com', N'nikolayk', N'pass123', 2, 1)
INSERT [dbo].[User] ([id], [lname], [fname], [patronymic], [dateBirth], [phone], [email], [login], [password], [roleId], [isActive]) VALUES (23, N'Лебедева', N'Екатерина', N'Анатольевна', CAST(N'1993-01-10' AS Date), N'12345678923', N'ekaterina.lebedeva@example.com', N'ekaterinal', N'pass123', 2, 1)
INSERT [dbo].[User] ([id], [lname], [fname], [patronymic], [dateBirth], [phone], [email], [login], [password], [roleId], [isActive]) VALUES (24, N'Белова', N'Андрей', N'Сергеевич', CAST(N'1987-02-15' AS Date), N'12345678924', N'andrey.belov@example.com', N'andreyb', N'pass123', 2, 1)
SET IDENTITY_INSERT [dbo].[User] OFF
GO
ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF_User_isActive]  DEFAULT ((1)) FOR [isActive]
GO
ALTER TABLE [dbo].[Client]  WITH CHECK ADD  CONSTRAINT [FK_Client_Role] FOREIGN KEY([roleId])
REFERENCES [dbo].[Role] ([id])
GO
ALTER TABLE [dbo].[Client] CHECK CONSTRAINT [FK_Client_Role]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_Product_Client] FOREIGN KEY([clientId])
REFERENCES [dbo].[Client] ([id])
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_Product_Client]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_Product_ProductStatus] FOREIGN KEY([statusId])
REFERENCES [dbo].[ProductStatus] ([id])
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_Product_ProductStatus]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_Product_ProductType] FOREIGN KEY([categoryId])
REFERENCES [dbo].[ProductType] ([id])
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_Product_ProductType]
GO
ALTER TABLE [dbo].[Return]  WITH CHECK ADD  CONSTRAINT [FK_Return_Product] FOREIGN KEY([productId])
REFERENCES [dbo].[Product] ([id])
GO
ALTER TABLE [dbo].[Return] CHECK CONSTRAINT [FK_Return_Product]
GO
ALTER TABLE [dbo].[Return]  WITH CHECK ADD  CONSTRAINT [FK_Return_ReturnType] FOREIGN KEY([returnTypeId])
REFERENCES [dbo].[ReturnType] ([id])
GO
ALTER TABLE [dbo].[Return] CHECK CONSTRAINT [FK_Return_ReturnType]
GO
ALTER TABLE [dbo].[Sales]  WITH CHECK ADD  CONSTRAINT [FK_Sales_Client] FOREIGN KEY([clientId])
REFERENCES [dbo].[Client] ([id])
GO
ALTER TABLE [dbo].[Sales] CHECK CONSTRAINT [FK_Sales_Client]
GO
ALTER TABLE [dbo].[Sales]  WITH CHECK ADD  CONSTRAINT [FK_Sales_PaymentMethod] FOREIGN KEY([payMethodId])
REFERENCES [dbo].[PaymentMethod] ([id])
GO
ALTER TABLE [dbo].[Sales] CHECK CONSTRAINT [FK_Sales_PaymentMethod]
GO
ALTER TABLE [dbo].[Sales]  WITH CHECK ADD  CONSTRAINT [FK_Sales_Product] FOREIGN KEY([productId])
REFERENCES [dbo].[Product] ([id])
GO
ALTER TABLE [dbo].[Sales] CHECK CONSTRAINT [FK_Sales_Product]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_Role1] FOREIGN KEY([roleId])
REFERENCES [dbo].[Role] ([id])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_Role1]
GO
