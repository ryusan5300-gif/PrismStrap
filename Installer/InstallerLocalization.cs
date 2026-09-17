using System;
using System.Collections.Generic;

namespace PrismStrap.Installer
{
    public static class InstallerLocalization
    {
        public static string CurrentLanguage { get; private set; } = "ja";

        public static readonly (string Code, string Label)[] SupportedLanguages = new[]
        {
            ("ja", "🇯🇵 日本語"),
            ("en", "🇺🇸 English"),
            ("ko", "🇰🇷 한국어"),
            ("id", "🇮🇩 Bahasa Indonesia"),
            ("zh", "🇨🇳 简体中文"),
            ("pt", "🇧🇷 Português")
        };

        private static readonly Dictionary<string, Dictionary<string, string>> Translations = new()
        {
            ["ja"] = new()
            {
                ["SetupTitle"] = "PrismStrap セットアップ",
                ["WelcomeHeader"] = "PrismStrap のインストール",
                ["WelcomeDesc"] = "Roblox体験を最適化する次世代ブートストラッパーをPCにセットアップします。",
                ["InstallDirLabel"] = "インストール先フォルダー:",
                ["Browse"] = "参照...",
                ["OptShortcuts"] = "デスクトップにショートカットを作成",
                ["OptStartMenu"] = "スタートメニューにショートカットを作成",
                ["OptProtocol"] = "Robloxの起動をPrismStrapに関連付ける (ブラウザ連携)",
                ["BtnInstall"] = "インストール",
                ["BtnCancel"] = "キャンセル",
                ["StatusExtracting"] = "ファイルを展開しています...",
                ["StatusShortcuts"] = "ショートカットを作成しています...",
                ["StatusRegistering"] = "システム設定を構成しています...",
                ["FinishTitle"] = "インストールが完了しました！",
                ["FinishDesc"] = "PrismStrap のセットアップが正常に完了しました。",
                ["LaunchNow"] = "PrismStrap を今すぐ起動する",
                ["BtnFinish"] = "完了"
            },
            ["en"] = new()
            {
                ["SetupTitle"] = "PrismStrap Setup",
                ["WelcomeHeader"] = "Install PrismStrap",
                ["WelcomeDesc"] = "Set up the next-generation Roblox bootstrapper & optimizer on your PC.",
                ["InstallDirLabel"] = "Installation Folder:",
                ["Browse"] = "Browse...",
                ["OptShortcuts"] = "Create Desktop shortcut",
                ["OptStartMenu"] = "Create Start Menu shortcut",
                ["OptProtocol"] = "Register as Roblox launcher (Browser Integration)",
                ["BtnInstall"] = "Install",
                ["BtnCancel"] = "Cancel",
                ["StatusExtracting"] = "Extracting application files...",
                ["StatusShortcuts"] = "Creating shortcuts...",
                ["StatusRegistering"] = "Configuring system integrations...",
                ["FinishTitle"] = "Installation Complete!",
                ["FinishDesc"] = "PrismStrap has been successfully installed.",
                ["LaunchNow"] = "Launch PrismStrap now",
                ["BtnFinish"] = "Finish"
            },
            ["ko"] = new()
            {
                ["SetupTitle"] = "PrismStrap 설치",
                ["WelcomeHeader"] = "PrismStrap 설치",
                ["WelcomeDesc"] = "Roblox 경험을 최적화하는 차세대 부트스트래퍼를 PC에 설치합니다.",
                ["InstallDirLabel"] = "설치 폴더:",
                ["Browse"] = "찾아보기...",
                ["OptShortcuts"] = "바탕화면에 바로가기 생성",
                ["OptStartMenu"] = "시작 메뉴에 바로가기 생성",
                ["OptProtocol"] = "Roblox 실행을 PrismStrap에 연결 (브라우저 연동)",
                ["BtnInstall"] = "설치",
                ["BtnCancel"] = "취소",
                ["StatusExtracting"] = "파일을 압축 해제하고 있습니다...",
                ["StatusShortcuts"] = "바로가기를 생성하고 있습니다...",
                ["StatusRegistering"] = "시스템 설정을 구성하고 있습니다...",
                ["FinishTitle"] = "설치가 완료되었습니다!",
                ["FinishDesc"] = "PrismStrap 설치가 성공적으로 완료되었습니다.",
                ["LaunchNow"] = "지금 PrismStrap 실행",
                ["BtnFinish"] = "완료"
            },
            ["id"] = new()
            {
                ["SetupTitle"] = "Pengaturan PrismStrap",
                ["WelcomeHeader"] = "Instal PrismStrap",
                ["WelcomeDesc"] = "Pasang bootstrapper & pengoptimal Roblox generasi berikutnya di PC Anda.",
                ["InstallDirLabel"] = "Folder Instalasi:",
                ["Browse"] = "Jelajahi...",
                ["OptShortcuts"] = "Buat pintasan di Desktop",
                ["OptStartMenu"] = "Buat pintasan di Menu Mulai",
                ["OptProtocol"] = "Daftarkan sebagai peluncur Roblox (Integrasi Browser)",
                ["BtnInstall"] = "Instal",
                ["BtnCancel"] = "Batal",
                ["StatusExtracting"] = "Mengekstrak file aplikasi...",
                ["StatusShortcuts"] = "Membuat pintasan...",
                ["StatusRegistering"] = "Mengonfigurasi pengaturan sistem...",
                ["FinishTitle"] = "Instalasi Selesai!",
                ["FinishDesc"] = "PrismStrap telah berhasil dipasang.",
                ["LaunchNow"] = "Luncurkan PrismStrap sekarang",
                ["BtnFinish"] = "Selesai"
            },
            ["zh"] = new()
            {
                ["SetupTitle"] = "PrismStrap 安装程序",
                ["WelcomeHeader"] = "安装 PrismStrap",
                ["WelcomeDesc"] = "在您的 PC 上安装用于优化 Roblox 体验的下一代引导程序。",
                ["InstallDirLabel"] = "安装文件夹:",
                ["Browse"] = "浏览...",
                ["OptShortcuts"] = "创建桌面快捷方式",
                ["OptStartMenu"] = "创建开始菜单快捷方式",
                ["OptProtocol"] = "关联 Roblox 启动到 PrismStrap (浏览器集成)",
                ["BtnInstall"] = "安装",
                ["BtnCancel"] = "取消",
                ["StatusExtracting"] = "正在提取应用程序文件...",
                ["StatusShortcuts"] = "正在创建快捷方式...",
                ["StatusRegistering"] = "正在配置系统设置...",
                ["FinishTitle"] = "安装完成！",
                ["FinishDesc"] = "PrismStrap 已成功安装。",
                ["LaunchNow"] = "立即启动 PrismStrap",
                ["BtnFinish"] = "完成"
            },
            ["pt"] = new()
            {
                ["SetupTitle"] = "Instalação do PrismStrap",
                ["WelcomeHeader"] = "Instalar o PrismStrap",
                ["WelcomeDesc"] = "Configure o bootstrapper e otimizador de nova geração para Roblox no seu PC.",
                ["InstallDirLabel"] = "Pasta de Instalação:",
                ["Browse"] = "Procurar...",
                ["OptShortcuts"] = "Criar atalho na Área de Trabalho",
                ["OptStartMenu"] = "Criar atalho no Menu Iniciar",
                ["OptProtocol"] = "Registrar como inicializador do Roblox (Integração com Navegador)",
                ["BtnInstall"] = "Instalar",
                ["BtnCancel"] = "Cancelar",
                ["StatusExtracting"] = "Extraindo arquivos do aplicativo...",
                ["StatusShortcuts"] = "Criando atalhos...",
                ["StatusRegistering"] = "Configurando integrações do sistema...",
                ["FinishTitle"] = "Instalação Concluída!",
                ["FinishDesc"] = "O PrismStrap foi instalado com sucesso.",
                ["LaunchNow"] = "Iniciar o PrismStrap agora",
                ["BtnFinish"] = "Concluir"
            }
        };

        public static event Action? LanguageChanged;

        public static void SetLanguage(string langCode)
        {
            if (Translations.ContainsKey(langCode))
            {
                CurrentLanguage = langCode;
                LanguageChanged?.Invoke();
            }
        }

        public static string Get(string key)
        {
            if (Translations.TryGetValue(CurrentLanguage, out var dict) && dict.TryGetValue(key, out var val))
            {
                return val;
            }
            if (Translations["en"].TryGetValue(key, out var fallback))
            {
                return fallback;
            }
            return key;
        }
    }
}
