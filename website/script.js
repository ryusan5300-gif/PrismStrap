// ==========================================================================
// PrismStrap Official Website - Scripts & Localization
// ==========================================================================

const i18n = {
  ja: {
    navFeatures: "機能",
    navWorkshop: "ワークショップ",
    navComparison: "比較",
    navHow: "導入方法",
    navFaq: "よくある質問",
    navDownload: "ダウンロード",
    
    badgeAnnouncement: "PrismStrap v0.3.0 正式リリース",
    heroTitle: "Roblox を、もっと自由に。<br><span class=\"text-gradient\">美しく、軽快に。</span>",
    heroSubtitle: "次世代の Roblox ブートストラッパー & 最適化ツール。FPS上限解除、カスタム死亡サウンド、グラフィック最適化、FastFlagsエディターを、美しい Windows 11 Fluent UI で。",
    btnDownloadSetup: "Setup.exe をダウンロード",
    btnDownloadZip: "ポータブル版 (ZIP)",
    btnGithub: "GitHub で見る",
    metaReq: "Windows 10 / 11 64bit 対応",
    metaNoWebview: "WebView2 不要 (100% Native)",
    metaSafe: "クライアント改造なしで安全",

    statOnlinePlayers: "Roblox プレイ中",
    statOnlineSub: "PrismStrap でオンライン",
    statOfflinePlayers: "待機中 (オフライン)",
    statOfflineSub: "インストール済みユーザー",
    statDownloads: "総ダウンロード数",
    statDownloadSub: "累計インストール",

    previewReadyBadge: "最新バージョン利用可能",
    previewHeroTitle: "最適化設定を適用して起動",
    previewHeroDesc: "FPS上限解除や指定したグラフィックス設定・死亡サウンドを反映した状態でRobloxを立ち上げます。",
    previewBtnPlay: "▶ 今すぐプレイ",
    previewStatFps: "FPS上限",
    previewStatGfx: "グラフィックAPI",
    previewStatSound: "死亡サウンド",
    previewStatProtocol: "ブラウザ連携",

    secFeaturesTag: "FEATURES",
    secFeaturesTitle: "ゲームプレイを別次元へ引き上げる機能群",
    secFeaturesDesc: "必要なすべての最適化を、ひとつの直感的で洗練されたアプリに凝縮。",
    
    featFpsTitle: "FPS 上限解除 (Unlocker)",
    featFpsDesc: "Robloxの標準60FPS制限を完全解除。120/144/240FPSや無制限に対応し、高リフレッシュレートモニターの性能を100%発揮します。",
    
    featSoundTitle: "カスタム死亡時サウンド",
    featSoundDesc: "伝説の元祖「OOF」音、迫力の「Vine Boom」、お好みの「カスタムMP3」を自由に設定。再生音量やピッチ連動速度も自在に調整可能。",
    
    featGfxTitle: "グラフィック & レンダリング",
    featGfxDesc: "Direct3D 11 や Vulkan をワンクリック選択。高解像度テクスチャの強制適用や不要なポストエフェクトの削減で軽快な描画を実現。",
    
    featFlagsTitle: "ビジュアル FastFlags エディター",
    featFlagsDesc: "危険で面倒な JSON 手動編集は不要。安全な GUI からフラグの追加・変更・削除をリアルタイムに行えます。",

    featLaunchTitle: "洗練されたロードUI & モード選択",
    featLaunchDesc: "ブラウザからの起動時も美しいスプラッシュダイアログを表示。起動時に「Robloxをプレイ」か「設定を開く」かを素早く選べます。",

    featNativeTitle: "100% C# .NET 8 WPF ネイティブ",
    featNativeDesc: "HTML/WebView2/ブラウザ依存を一切排除。純粋なネイティブコードでビルドされ、極めて軽量かつ超高速に応答します。",

    secCompTag: "COMPARISON",
    secCompTitle: "標準ランチャーとの違い",
    secCompDesc: "標準のRobloxランチャーでは味わえない、究極のカスタマイズ性と快適性。",
    compColFeature: "機能 / 項目",
    compColStandard: "標準ランチャー",
    compColPrism: "PrismStrap",
    compRow1: "FPS 制限解除 (無制限)",
    compRow2: "死亡サウンド変更 (OOF/Vine Boom/MP3)",
    compRow3: "サウンドの音量・再生速度調整",
    compRow4: "グラフィック API 選択 (Vulkan/D3D11)",
    compRow5: "FastFlags ビジュアル編集",
    compRow6: "ブラウザ連携スプラッシュロードUI",
    compRow7: "多言語ネイティブUI (6言語)",
    compRow8: "単一スタンドアロン Setup.exe",

    secStepsTag: "HOW IT WORKS",
    secStepsTitle: "導入はわずか3ステップ",
    step1Title: "Setup.exe をダウンロード",
    step1Desc: "ワンクリックでインストールが完了。ショートカットやブラウザ連携も自動で構成されます。",
    step2Title: "好みに合わせて最適化を設定",
    step2Desc: "FPS上限や死亡サウンド、グラフィック設定をお好みに合わせてカスタマイズ。",
    step3Title: "いつも通り Roblox をプレイ",
    step3Desc: "ブラウザやデスクトップからゲームを起動するだけで、すべての最適化が自動適用されます。",

    secFaqTag: "FAQ",
    secFaqTitle: "よくある質問",
    faq1Q: "PrismStrap を使用してアカウントが BAN される心配はありますか？",
    faq1A: "心配ありません。PrismStrap はゲームのメモリ改変やチート行為を一切行いません。Roblox公式が提供している ClientSettings (FastFlags) やオーディオアセットの安全な構成のみを行うため、規約に違反せず安全にご利用いただけます。",
    faq2Q: "動作に必要な環境やシステム要件は？",
    faq2A: "Windows 10 / 11 (64bit) に対応しています。.NET 8 ランタイム環境で動作し、WebView2 などの重い外部コンポーネントは不要です。",
    faq3Q: "アンインストールはどのように行いますか？",
    faq3A: "Windows の「設定」>「アプリ」>「インストールされているアプリ」から、通常のアプリと同様にワンクリックで完全にアンインストール可能です。",
    faq4Q: "ブラウザからRobloxを起動した時も適用されますか？",
    faq4A: "はい！インストーラーで「ブラウザ連携」を有効にしておくことで、RobloxのWebサイト上の「Play」ボタンをクリックした際も PrismStrap が自動起動し、すべての最適化が反映されます。",

    ctaTitle: "今すぐ、最高の Roblox 体験を。",
    ctaDesc: "セットアップは数秒。完全に無料で、広告もありません。",
    footerDisclaimer: "免責事項: PrismStrap は独立したオープンソースプロジェクトであり、Roblox Corporation との提携、承認、関連付けは一切ありません。「Roblox」は Roblox Corporation の登録商標です。"
  },
  en: {
    navFeatures: "Features",
    navWorkshop: "Workshop",
    navComparison: "Comparison",
    navHow: "How it works",
    navFaq: "FAQ",
    navDownload: "Download",
    
    badgeAnnouncement: "PrismStrap v0.3.0 Available now",
    heroTitle: "Roblox, Elevated.<br><span class=\"text-gradient\">Faster. Smoother. Smarter.</span>",
    heroSubtitle: "The next-generation Roblox bootstrapper & optimizer. Unlock your FPS, customize death sounds, optimize graphics, and manage FastFlags in a stunning Windows 11 Fluent UI.",
    btnDownloadSetup: "Download Setup.exe",
    btnDownloadZip: "Portable (ZIP)",
    btnGithub: "View on GitHub",
    metaReq: "Windows 10 / 11 64-bit",
    metaNoWebview: "No WebView2 (100% Native)",
    metaSafe: "Safe & Non-intrusive",

    statOnlinePlayers: "Playing Roblox",
    statOnlineSub: "Online with PrismStrap",
    statOfflinePlayers: "Idle (Offline)",
    statOfflineSub: "Installed Users",
    statDownloads: "Total Downloads",
    statDownloadSub: "Total Installs",

    previewReadyBadge: "Latest version ready",
    previewHeroTitle: "Launch with Optimizations",
    previewHeroDesc: "Launch Roblox with uncapped FPS, custom graphics APIs, and tailored death sounds applied automatically.",
    previewBtnPlay: "▶ Play Now",
    previewStatFps: "FPS Limit",
    previewStatGfx: "Graphics API",
    previewStatSound: "Death Sound",
    previewStatProtocol: "Browser Integration",

    secFeaturesTag: "FEATURES",
    secFeaturesTitle: "Engineered for Peak Performance",
    secFeaturesDesc: "Every essential tweak and enhancement packed into one intuitive, elegant utility.",
    
    featFpsTitle: "Uncapped Frame Rates",
    featFpsDesc: "Bypass Roblox's default 60 FPS cap. Choose 120, 144, 240, or fully unlimited to unlock your monitor's true potential.",
    
    featSoundTitle: "Custom Death Sounds",
    featSoundDesc: "Relive the legendary classic 'OOF', trigger the punchy 'Vine Boom', or load your own custom MP3s with pitch-synchronized volume & speed controls.",
    
    featGfxTitle: "Graphics & Rendering",
    featGfxDesc: "Effortlessly toggle between Vulkan and Direct3D 11. Force ultra-crisp texture overrides and disable heavy post-processing effects.",
    
    featFlagsTitle: "Visual FastFlags Editor",
    featFlagsDesc: "No more risky, tedious JSON editing. Tweak, add, and manage FastFlags with safety and ease directly from the GUI.",

    featLaunchTitle: "Launch Splash & Mode Select",
    featLaunchDesc: "Enjoy a sleek Fluent splash dialog when launching games from your browser. Pick between jumping straight in or tweaking settings on launch.",

    featNativeTitle: "100% C# .NET 8 WPF Native",
    featNativeDesc: "Zero HTML or WebView2 bloat. Built natively from the ground up for minimal memory usage and lightning-fast responsiveness.",

    secCompTag: "COMPARISON",
    secCompTitle: "Standard Launcher vs PrismStrap",
    secCompDesc: "See what sets PrismStrap apart from the official stock experience.",
    compColFeature: "Feature",
    compColStandard: "Stock Launcher",
    compColPrism: "PrismStrap",
    compRow1: "FPS Uncapping (Unlimited)",
    compRow2: "Death Sound Customization (OOF / Vine Boom / MP3)",
    compRow3: "Sound Volume & Pitch/Speed Controls",
    compRow4: "Graphics API Selection (Vulkan / D3D11)",
    compRow5: "Visual FastFlags GUI Editor",
    compRow6: "Browser-integrated Splash Loading UI",
    compRow7: "Multi-language Native UI (6 Languages)",
    compRow8: "Single Standalone Setup.exe Installer",

    secStepsTag: "HOW IT WORKS",
    secStepsTitle: "Get Started in 3 Simple Steps",
    step1Title: "Download Setup.exe",
    step1Desc: "One-click install configures shortcuts and browser integration automatically in seconds.",
    step2Title: "Customize Your Settings",
    step2Desc: "Set your target FPS, pick your favorite death sound, and fine-tune graphics options.",
    step3Title: "Play Roblox as Usual",
    step3Desc: "Hit 'Play' on the Roblox website or your desktop icon. All your mods apply seamlessly.",

    secFaqTag: "FAQ",
    secFaqTitle: "Frequently Asked Questions",
    faq1Q: "Can I get banned for using PrismStrap?",
    faq1A: "No. PrismStrap does not inject into game memory or modify gameplay code. It merely configures official Roblox ClientSettings (FastFlags) and user asset files, fully adhering to standard usage.",
    faq2Q: "What are the system requirements?",
    faq2A: "Compatible with Windows 10 and 11 (64-bit). Runs natively on .NET 8 without any heavy external runtimes or WebView2 dependencies.",
    faq3Q: "How do I uninstall PrismStrap?",
    faq3A: "You can remove it anytime directly from Windows Settings > Apps > Installed Apps with a single click.",
    faq4Q: "Does it work when launching games from a web browser?",
    faq4A: "Absolutely! With browser integration enabled during setup, clicking 'Play' on roblox.com automatically launches PrismStrap's splash dialog and applies your mods.",

    ctaTitle: "Elevate Your Roblox Experience Today.",
    ctaDesc: "Takes only seconds to set up. Free, open source, and zero ads.",
    footerDisclaimer: "Disclaimer: PrismStrap is an independent open-source project and is not affiliated with, endorsed by, or associated with Roblox Corporation. 'Roblox' is a registered trademark of Roblox Corporation."
  }
};

let currentLang = 'ja';

function setLanguage(lang) {
  currentLang = lang;
  localStorage.setItem('prismstrap_lang', lang);
  
  const dict = i18n[lang] || i18n.ja;
  
  // Update elements with data-i18n
  document.querySelectorAll('[data-i18n]').forEach(el => {
    const key = el.getAttribute('data-i18n');
    if (dict[key]) {
      el.innerHTML = dict[key];
    }
  });

  // Update language toggle button label
  const btnLang = document.getElementById('btnLang');
  if (btnLang) {
    btnLang.innerHTML = lang === 'ja' ? '🌐 English' : '🌐 日本語';
  }
}

document.addEventListener('DOMContentLoaded', () => {
  // Load stored language or fallback
  const stored = localStorage.getItem('prismstrap_lang');
  if (stored && (stored === 'ja' || stored === 'en')) {
    setLanguage(stored);
  } else {
    // Detect browser language
    const navLang = (navigator.language || '').toLowerCase();
    setLanguage(navLang.startsWith('ja') ? 'ja' : 'en');
  }

  // Language Toggle Button
  const btnLang = document.getElementById('btnLang');
  if (btnLang) {
    btnLang.addEventListener('click', () => {
      setLanguage(currentLang === 'ja' ? 'en' : 'ja');
    });
  }

  // FAQ Accordion
  document.querySelectorAll('.faq-question').forEach(button => {
    button.addEventListener('click', () => {
      const item = button.closest('.faq-item');
      const isActive = item.classList.contains('active');
      
      // Close all other items
      document.querySelectorAll('.faq-item').forEach(i => i.classList.remove('active'));
      
      if (!isActive) {
        item.classList.add('active');
      }
    });
  });

  // Smooth scroll for internal links
  document.querySelectorAll('a[href^="#"]').forEach(anchor => {
    anchor.addEventListener('click', function (e) {
      e.preventDefault();
      const targetId = this.getAttribute('href').substring(1);
      const target = document.getElementById(targetId);
      if (target) {
        target.scrollIntoView({ behavior: 'smooth', block: 'start' });
      }
    });
  });

  // Initialize live community player activity
  initLiveStats();
});

// ==========================================================================
// Live Activity Stats Tracker (Real CountAPI Integration)
// ==========================================================================
const COUNTAPI_BASE = 'https://countapi.mileshilliard.com/api/v1';
const KEY_DOWNLOADS = 'prismstrap_v1_total_downloads';
const KEY_ONLINE = 'prismstrap_v1_active_players';

let currentOnline = 0;
let currentOffline = 0;
let currentDownloads = 0;

function updateLiveStatsUI() {
  const onlineEl = document.getElementById('liveOnlineCount');
  const offlineEl = document.getElementById('liveOfflineCount');
  const dlEl = document.getElementById('liveDownloadCount');

  if (onlineEl) onlineEl.textContent = currentOnline.toLocaleString();
  if (offlineEl) offlineEl.textContent = currentOffline.toLocaleString();
  if (dlEl) dlEl.textContent = currentDownloads.toLocaleString();
}

async function fetchLiveStats() {
  try {
    const [dlRes, onlineRes] = await Promise.allSettled([
      fetch(`${COUNTAPI_BASE}/get/${KEY_DOWNLOADS}`).then(r => r.json()),
      fetch(`${COUNTAPI_BASE}/get/${KEY_ONLINE}`).then(r => r.json())
    ]);

    if (dlRes.status === 'fulfilled' && typeof dlRes.value?.value === 'number') {
      currentDownloads = dlRes.value.value;
    }
    if (onlineRes.status === 'fulfilled' && typeof onlineRes.value?.value === 'number') {
      currentOnline = onlineRes.value.value;
    }

    currentOffline = Math.max(0, currentDownloads - currentOnline);
    updateLiveStatsUI();
  } catch (err) {
    console.warn('[LiveStats] Failed to fetch real stats:', err);
  }
}

async function recordDownloadHit() {
  try {
    const res = await fetch(`${COUNTAPI_BASE}/hit/${KEY_DOWNLOADS}`);
    const data = await res.json();
    if (typeof data.value === 'number') {
      currentDownloads = data.value;
      currentOffline = Math.max(0, currentDownloads - currentOnline);
      updateLiveStatsUI();
    }
  } catch (err) {
    console.warn('[LiveStats] Failed to record download hit:', err);
  }
}

function initLiveStats() {
  updateLiveStatsUI();
  fetchLiveStats();

  // Attach real download hit tracking to download buttons
  const downloadLinks = document.querySelectorAll('a[href*="Setup.exe"], a[href*=".zip"]');
  downloadLinks.forEach(link => {
    link.addEventListener('click', () => {
      recordDownloadHit();
    });
  });

  // Periodically refresh real stats from CountAPI every 30 seconds (No dummy random numbers!)
  setInterval(fetchLiveStats, 30000);
}

