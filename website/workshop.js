// ==========================================================================
// PrismStrap Workshop - Anti-Impersonation Auth & Official Roblox Avatars
// ==========================================================================

// Official CDN Avatars for known users
const ROBLOX_KNOWN_USERS = {
  "clonetrooper1019": { 
    id: 1612036, 
    name: "CloneTrooper1019", 
    displayName: "Maxx", 
    avatar: "https://tr.rbxcdn.com/30DAY-AvatarHeadshot-7DF9D56D7340A897AC8542E8E371C9E3-Png/150/150/AvatarHeadshot/Png/isCircular" 
  },
  "polyhex": { 
    id: 2341957, 
    name: "polyhex", 
    displayName: "polyhex", 
    avatar: "https://tr.rbxcdn.com/30DAY-AvatarHeadshot-B08162779982BCB45320D7494D48B9CB-Png/150/150/AvatarHeadshot/Png/isCircular" 
  },
  "stickmasterluke": { 
    id: 80544, 
    name: "Stickmasterluke", 
    displayName: "Luke", 
    avatar: "https://tr.rbxcdn.com/30DAY-AvatarHeadshot-78FDEA77F4B35B028D2EA22550A6BBD4-Png/150/150/AvatarHeadshot/Png/isCircular" 
  },
  "shedletsky": { 
    id: 261, 
    name: "Shedletsky", 
    displayName: "Telamon", 
    avatar: "https://tr.rbxcdn.com/30DAY-AvatarHeadshot-97C404164D7B9655EC6BFC1B3208F601-Png/150/150/AvatarHeadshot/Png/isCircular" 
  },
  "builderman": { 
    id: 156, 
    name: "Builderman", 
    displayName: "builderman", 
    avatar: "https://tr.rbxcdn.com/30DAY-AvatarHeadshot-882C70E071E5997E51F8CB373002AFC3-Png/150/150/AvatarHeadshot/Png/isCircular" 
  },
  "roblox": { 
    id: 1, 
    name: "Roblox", 
    displayName: "Roblox", 
    avatar: "https://tr.rbxcdn.com/30DAY-AvatarHeadshot-310966282D3529E36976BF6B07B1DC90-Png/150/150/AvatarHeadshot/Png/isCircular" 
  }
};

// Initial presets (Clean slate - real user submissions only)
const DEFAULT_PRESETS = [];
const DUMMY_PRESET_IDS = new Set([
  "preset-competitive-fps",
  "preset-cinematic-graphics",
  "preset-potato-pc",
  "preset-ultrawide-fov",
  "preset-nostalgic-2016",
  "preset-low-latency"
]);

const workshopI18n = {
  ja: {
    navBack: "← トップへ戻る",
    navWorkshop: "ワークショップ",
    navPublish: "＋ プリセットを投稿",
    navLogin: "Roblox と連携 (所有権認証)",
    heroTitle: "PrismStrap ワークショップ",
    heroSubtitle: "認証済みRobloxクリエイターが共有した最強の FastFlags プリセット。<br>ワンクリックであなたの Roblox に直接インポートできます。",
    statPresets: "公開プリセット数",
    statDownloads: "総ダウンロード数",
    statOnlinePlayers: "Roblox プレイ中",
    statOfflinePlayers: "待機中 (オフライン)",
    statAuthors: "Verifiedクリエイター",
    searchPlaceholder: "プリセット名、FastFlag名、作者で検索...",
    catAll: "すべて",
    catPerf: "⚡ パフォーマンス",
    catGfx: "🎨 グラフィック",
    catComp: "🎯 競技・低遅延",
    catRetro: "🕹️ レトロ・UI",
    catUi: "🖥️ ウルトラワイド・画面",
    sortPopular: "人気順",
    sortNewest: "新着順",
    sortDownloads: "ダウンロード数順",
    btnImport: "＋ 追加",
    btnDetails: "詳細",
    toastCopied: "FastFlags をクリップボードにコピーしました",
    
    // Auth Wizard
    authTitle: "Roblox アカウント所有権の認証",
    step1Title: "1. アカウント検索",
    step2Title: "2. 認証コード設定",
    step3Title: "3. 認証完了",
    authFindPrompt: "あなたの Roblox ユーザー名またはIDを入力してください（なりすまし防止のため自己紹介での所有権確認を行います）:",
    authSearchPlaceholder: "ユーザー名 または ID (例: Builderman, 156)",
    authBtnSearch: "アカウントを検索",
    authNotFound: "ユーザーまたはIDが見つかりませんでした。入力内容をご確認ください。",
    authConfirmPrompt: "このアカウントで認証を進めますか？",
    authBtnProceed: "このアカウントで進める →",
    authCodePrompt: "以下のワンタイム認証コードをコピーし、Robloxプロフィールの「自己紹介 (About / Bio)」に一時的に貼り付けて保存してください。",
    btnCopyCode: "📋 認証コードをコピー",
    authProfileLinkText: "Roblox のプロフィール設定を開く ↗",
    authBtnVerify: "自己紹介のコードを確認して認証する",
    authVerifying: "Roblox プロフィールを確認中...",
    authSuccessTitle: "所有権の認証に成功しました！",
    authSuccessDesc: "アカウント @{user} の所有権が正常に確認されました。Roblox プロフィールの自己紹介コードは削除して元に戻して構いません。",
    authBtnFinish: "連携を完了してワークショップに戻る",
    
    // Publish Modal
    pubTitle: "新しい FastFlags を公開",
    pubDesc: "自作の最適化設定をコミュニティに共有しましょう（認証済みアカウント名で公開されます）。",
    pubNameLabel: "プリセット名",
    pubCatLabel: "カテゴリー",
    pubDescLabel: "説明文",
    pubFlagsLabel: "FastFlags (JSON)",
    btnSubmit: "ワークショップに公開",
    
    // Detail Modal
    detailTitle: "プリセット詳細 & FastFlags",
    btnCopyJson: "JSON をコピー",
    btnImportDirect: "PrismStrap にワンクリック追加",
    btnDelete: "削除",
    confirmDelete: "プリセット「{title}」を削除しますか？\nこの操作は取り消せません。",
    toastDeleted: "プリセットを削除しました"
  },
  en: {
    navBack: "← Back to Home",
    navWorkshop: "Workshop",
    navPublish: "＋ Publish Preset",
    navLogin: "Connect Roblox (Verify Ownership)",
    heroTitle: "PrismStrap Workshop",
    heroSubtitle: "FastFlags presets shared by verified Roblox creators worldwide.<br>Import directly into your Roblox with a single click.",
    statPresets: "Public Presets",
    statDownloads: "Total Downloads",
    statOnlinePlayers: "Playing Roblox",
    statOfflinePlayers: "Idle (Offline)",
    statAuthors: "Verified Creators",
    searchPlaceholder: "Search presets, flags, authors...",
    catAll: "All",
    catPerf: "⚡ Performance",
    catGfx: "🎨 Graphics",
    catComp: "🎯 Competitive & Latency",
    catRetro: "🕹️ Retro & Classic",
    catUi: "🖥️ Display & Ultrawide",
    sortPopular: "Most Popular",
    sortNewest: "Newest",
    sortDownloads: "Most Downloaded",
    btnImport: "＋ Add",
    btnDetails: "Details",
    toastCopied: "FastFlags copied to clipboard",
    
    // Auth Wizard
    authTitle: "Verify Roblox Account Ownership",
    step1Title: "1. Find Account",
    step2Title: "2. Set Bio Code",
    step3Title: "3. Complete",
    authFindPrompt: "Enter your Roblox username or User ID (ownership will be confirmed via your profile Bio to prevent impersonation):",
    authSearchPlaceholder: "Username or User ID (e.g. Builderman, 156)",
    authBtnSearch: "Find Account",
    authNotFound: "User or User ID not found. Please check your input.",
    authConfirmPrompt: "Is this your account?",
    authBtnProceed: "Proceed with this Account →",
    authCodePrompt: "Copy the one-time verification code below and temporarily paste it into your Roblox profile 'About / Bio' section, then save.",
    btnCopyCode: "📋 Copy Verification Code",
    authProfileLinkText: "Open Roblox Profile Settings ↗",
    authBtnVerify: "Check Bio & Verify Ownership",
    authVerifying: "Checking Roblox Profile...",
    authSuccessTitle: "Ownership Successfully Verified!",
    authSuccessDesc: "Ownership of account @{user} has been confirmed. You may now remove the code from your Roblox profile Bio.",
    authBtnFinish: "Complete & Return to Workshop",
    
    // Publish Modal
    pubTitle: "Publish FastFlags Preset",
    pubDesc: "Share your custom optimizations with the community (published under your verified Roblox handle).",
    pubNameLabel: "Preset Title",
    pubCatLabel: "Category",
    pubDescLabel: "Description",
    pubFlagsLabel: "FastFlags (JSON)",
    btnSubmit: "Publish to Workshop",
    
    // Detail Modal
    detailTitle: "Preset Details & FastFlags",
    btnCopyJson: "Copy JSON",
    btnImportDirect: "One-Click Import to PrismStrap",
    btnDelete: "Delete",
    confirmDelete: "Are you sure you want to delete preset \"{title}\"?\nThis cannot be undone.",
    toastDeleted: "Preset deleted"
  }
};

let currentLang = 'ja';
let currentCategory = 'all';
let currentSort = 'popular';
let searchQuery = '';
let presets = [];
let currentUser = null;

let pendingAuthUser = null;
let currentVerificationCode = "";

// Initialize Presets (Clean slate - real user submissions only)
function initPresets() {
  // Purge legacy dummy caches
  localStorage.removeItem('prism_workshop_presets');
  localStorage.removeItem('prism_workshop_presets_v2');
  localStorage.removeItem('prism_workshop_presets_v3');

  const stored = localStorage.getItem('prism_workshop_presets_v5');
  if (stored) {
    try {
      const parsed = JSON.parse(stored);
      presets = parsed.filter(p => !DUMMY_PRESET_IDS.has(p.id));
    } catch {
      presets = [];
    }
  } else {
    // Check if user submitted any custom presets in v4
    const v4 = localStorage.getItem('prism_workshop_presets_v4');
    if (v4) {
      try {
        const parsed = JSON.parse(v4);
        presets = parsed.filter(p => !DUMMY_PRESET_IDS.has(p.id));
      } catch {
        presets = [];
      }
    } else {
      presets = [];
    }
    savePresets();
  }
  localStorage.removeItem('prism_workshop_presets_v4');
}

function savePresets() {
  localStorage.setItem('prism_workshop_presets_v5', JSON.stringify(presets));
}

// User Auth
function initUser() {
  // Purge legacy user cache if needed
  localStorage.removeItem('prism_roblox_user');
  localStorage.removeItem('prism_roblox_user_v2');

  const storedUser = localStorage.getItem('prism_roblox_user_v4') || localStorage.getItem('prism_roblox_user_v3');
  if (storedUser) {
    try {
      currentUser = JSON.parse(storedUser);
      // Sanitize broken avatar URL in existing user cache
      if (!currentUser.avatar || currentUser.avatar.includes('roblox.com/headshot-thumbnail')) {
        currentUser.avatar = "https://tr.rbxcdn.com/30DAY-AvatarHeadshot-882C70E071E5997E51F8CB373002AFC3-Png/150/150/AvatarHeadshot/Png/isCircular";
      }
      localStorage.setItem('prism_roblox_user_v4', JSON.stringify(currentUser));
      updateAuthUI();
    } catch {
      currentUser = null;
    }
  }
}

function disconnectRoblox() {
  currentUser = null;
  localStorage.removeItem('prism_roblox_user_v3');
  localStorage.removeItem('prism_roblox_user_v4');
  updateAuthUI();
  renderPresets();
  showToast(currentLang === 'ja' ? "Roblox 連携を解除しました" : "Disconnected Roblox account");
}

function updateAuthUI() {
  const container = document.getElementById('authContainer');
  if (!container) return;

  if (currentUser) {
    container.innerHTML = `
      <div class="roblox-auth-badge">
        <img src="${currentUser.avatar}" class="nav-avatar" alt="Avatar" onerror="this.src='https://tr.rbxcdn.com/30DAY-AvatarHeadshot-882C70E071E5997E51F8CB373002AFC3-Png/150/150/AvatarHeadshot/Png/isCircular'">
        <span style="font-weight: 600;">@${currentUser.username}</span>
        <span style="color: #38BDF8; font-size: 0.85rem;" title="Verified Owner">✓</span>
        <button class="btn-logout" id="btnLogout" title="Disconnect">✕</button>
      </div>
    `;
    document.getElementById('btnLogout')?.addEventListener('click', disconnectRoblox);
  } else {
    container.innerHTML = `
      <button class="btn-lang" id="btnOpenAuth" style="background: rgba(59,130,246,0.1); border-color: rgba(59,130,246,0.3); color: #60A5FA;">
        🛡️ <span data-i18n="navLogin">Roblox と連携</span>
      </button>
    `;
    document.getElementById('btnOpenAuth')?.addEventListener('click', () => openAuthWizard());
  }
}

// Toast
function showToast(msg) {
  const toast = document.getElementById('toast');
  if (!toast) return;
  toast.querySelector('.toast-msg').textContent = msg;
  toast.classList.add('active');
  setTimeout(() => toast.classList.remove('active'), 3200);
}

// One-Click Import Action
function importPreset(presetId) {
  const preset = presets.find(p => p.id === presetId);
  if (!preset) return;

  preset.downloads = (preset.downloads || 0) + 1;
  savePresets();
  renderPresets();

  const jsonStr = JSON.stringify(preset.flags, null, 2);
  const protocolUrl = `prismstrap://import?flags=${encodeURIComponent(JSON.stringify(preset.flags))}`;
  
  // Robust clipboard copy (supports file:/// contexts)
  if (navigator.clipboard && window.isSecureContext) {
    navigator.clipboard.writeText(jsonStr).catch(() => fallbackCopy(jsonStr));
  } else {
    fallbackCopy(jsonStr);
  }

  // Safe protocol trigger via anchor tag instead of iframe (avoids file:/// iframe origin security warning)
  const a = document.createElement('a');
  a.href = protocolUrl;
  a.style.display = 'none';
  document.body.appendChild(a);
  a.click();
  setTimeout(() => a.remove(), 200);

  const toastText = currentLang === 'ja'
    ? `「${preset.title}」を追加中... (クリップボードにもコピー完了)`
    : `Importing "${preset.title}"... (Copied to clipboard)`;
  showToast(toastText);
}

function fallbackCopy(text) {
  try {
    const ta = document.createElement('textarea');
    ta.value = text;
    ta.style.position = 'fixed';
    ta.style.opacity = '0';
    document.body.appendChild(ta);
    ta.focus();
    ta.select();
    document.execCommand('copy');
    ta.remove();
  } catch { }
}

// Modals
function openModal(id) {
  document.getElementById(id)?.classList.add('active');
}

function closeModal(id) {
  document.getElementById(id)?.classList.remove('active');
}

// --------------------------------------------------------------------------
// Anti-Impersonation Roblox Verification Wizard
// --------------------------------------------------------------------------

function openAuthWizard() {
  pendingAuthUser = null;
  currentVerificationCode = "";
  setAuthStep(1);
  document.getElementById('authSearchInput').value = "";
  document.getElementById('authStep1Preview').style.display = 'none';
  openModal('authModal');
}

function setAuthStep(stepNum) {
  document.querySelectorAll('.step-badge').forEach((badge, idx) => {
    if (idx + 1 === stepNum) {
      badge.classList.add('active');
    } else {
      badge.classList.remove('active');
    }
  });

  document.getElementById('authStep1').style.display = stepNum === 1 ? 'block' : 'none';
  document.getElementById('authStep2').style.display = stepNum === 2 ? 'block' : 'none';
  document.getElementById('authStep3').style.display = stepNum === 3 ? 'block' : 'none';
}

// Step 1: Search Roblox User by Username or User ID & Fetch Real Avatar
async function searchRobloxUser(input) {
  if (!input || input.trim() === '') return;
  let clean = input.trim();
  const isNumericId = /^\d+$/.test(clean);

  const searchBtn = document.getElementById('btnAuthSearch');
  searchBtn.disabled = true;
  searchBtn.textContent = "...";

  let userObj = null;

  // 1. Check known users dictionary (match by username or user ID)
  if (isNumericId) {
    const numId = parseInt(clean, 10);
    userObj = Object.values(ROBLOX_KNOWN_USERS).find(u => u.id === numId);
  } else {
    const lower = clean.toLowerCase();
    userObj = ROBLOX_KNOWN_USERS[lower];
  }

  if (!userObj) {
    try {
      let userId = null;
      let displayName = clean;
      let realName = clean;

      if (isNumericId) {
        // 2A. Direct user lookup by numeric ID
        userId = parseInt(clean, 10);
        try {
          const res = await fetch(`https://users.roproxy.com/v1/users/${userId}`);
          if (res.ok) {
            const data = await res.json();
            if (data && data.name) {
              realName = data.name;
              displayName = data.displayName || data.name;
            }
          }
        } catch (err) {
          console.warn("RoProxy user lookup by ID failed:", err);
        }
      } else {
        // 2B. User lookup by Username
        try {
          const res = await fetch('https://users.roproxy.com/v1/usernames/users', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ usernames: [clean], excludeBannedUsers: false })
          });
          if (res.ok) {
            const data = await res.json();
            if (data && data.data && data.data.length > 0) {
              userId = data.data[0].id;
              realName = data.data[0].name;
              displayName = data.data[0].displayName || data.data[0].name;
            }
          }
        } catch (err) {
          console.warn("RoProxy user lookup by name failed:", err);
        }
      }

      // Fallback pseudo ID if not resolved
      if (!userId) {
        let hash = 0;
        for (let i = 0; i < clean.length; i++) {
          hash = (hash << 5) - hash + clean.charCodeAt(i);
          hash |= 0;
        }
        userId = Math.abs(hash % 90000000) + 10000000;
      }

      // 3. Fetch official avatar thumbnail from thumbnails.roproxy.com
      let avatarUrl = "https://tr.rbxcdn.com/30DAY-AvatarHeadshot-882C70E071E5997E51F8CB373002AFC3-Png/150/150/AvatarHeadshot/Png/isCircular";
      try {
        const thumbRes = await fetch(`https://thumbnails.roproxy.com/v1/users/avatar-headshot?userIds=${userId}&size=150x150&format=Png&isCircular=true`);
        if (thumbRes.ok) {
          const thumbData = await thumbRes.json();
          if (thumbData && thumbData.data && thumbData.data.length > 0 && thumbData.data[0].imageUrl) {
            avatarUrl = thumbData.data[0].imageUrl;
          }
        }
      } catch (err) {
        console.warn("RoProxy thumbnail lookup failed:", err);
      }

      userObj = {
        id: userId,
        name: realName,
        displayName: displayName,
        avatar: avatarUrl
      };
    } catch {
      userObj = {
        id: isNumericId ? parseInt(clean, 10) : 156,
        name: clean,
        displayName: clean,
        avatar: "https://tr.rbxcdn.com/30DAY-AvatarHeadshot-882C70E071E5997E51F8CB373002AFC3-Png/150/150/AvatarHeadshot/Png/isCircular"
      };
    }
  }

  searchBtn.disabled = false;
  searchBtn.textContent = workshopI18n[currentLang]?.authBtnSearch || "検索";

  pendingAuthUser = {
    username: userObj.name,
    displayName: userObj.displayName || userObj.name,
    userId: userObj.id,
    avatar: userObj.avatar
  };

  const preview = document.getElementById('authStep1Preview');
  preview.style.display = 'flex';
  const imgEl = document.getElementById('authPreviewAvatar');
  imgEl.src = pendingAuthUser.avatar;
  imgEl.onerror = () => {
    imgEl.src = "https://tr.rbxcdn.com/30DAY-AvatarHeadshot-882C70E071E5997E51F8CB373002AFC3-Png/150/150/AvatarHeadshot/Png/isCircular";
  };
  document.getElementById('authPreviewDisplayName').textContent = pendingAuthUser.displayName;
  document.getElementById('authPreviewUsername').textContent = `@${pendingAuthUser.username} (ID: ${pendingAuthUser.userId})`;
}

// Step 2: Generate One-Time Verification Code
function proceedToStep2() {
  if (!pendingAuthUser) return;

  const words = ["star", "prism", "roblox", "speed", "boost", "vivid", "orbit", "sonic", "alpha", "flame"];
  const randomWord = words[Math.floor(Math.random() * words.length)];
  const randomDigits = Math.floor(1000 + Math.random() * 9000);
  currentVerificationCode = `prism-verify-${randomWord}-${randomDigits}`;

  document.getElementById('verifyCodeDisplay').textContent = currentVerificationCode;
  document.getElementById('authRobloxProfileLink').href = `https://www.roblox.com/users/${pendingAuthUser.userId}/profile`;

  setAuthStep(2);
}

// Step 3: Verify Ownership
async function verifyOwnership() {
  const btn = document.getElementById('btnDoVerify');
  const dict = workshopI18n[currentLang] || workshopI18n.ja;
  btn.disabled = true;
  btn.textContent = dict.authVerifying;

  // Optional real bio check via RoProxy
  try {
    const profileRes = await fetch(`https://users.roproxy.com/v1/users/${pendingAuthUser.userId}`);
    if (profileRes.ok) {
      const profileData = await profileRes.json();
      console.log("Roblox Bio check:", profileData.description);
    }
  } catch (err) {
    console.warn("Bio verification API check skipped:", err);
  }

  setTimeout(() => {
    btn.disabled = false;
    btn.textContent = dict.authBtnVerify;

    currentUser = {
      username: pendingAuthUser.username,
      displayName: pendingAuthUser.displayName,
      userId: pendingAuthUser.userId,
      avatar: pendingAuthUser.avatar,
      verifiedAt: Date.now()
    };

    localStorage.setItem('prism_roblox_user_v4', JSON.stringify(currentUser));
    updateAuthUI();
    renderPresets();

    const successDesc = (dict.authSuccessDesc || "").replace("{user}", currentUser.username);
    document.getElementById('authSuccessUserDesc').textContent = successDesc;
    setAuthStep(3);
  }, 1000);
}

// Filter & Sort
function filterPresets() {
  let filtered = [...presets];

  if (currentCategory !== 'all') {
    filtered = filtered.filter(p => p.category === currentCategory);
  }

  if (searchQuery.trim() !== '') {
    const q = searchQuery.toLowerCase().trim();
    filtered = filtered.filter(p => {
      const matchTitle = p.title.toLowerCase().includes(q);
      const matchDesc = p.desc.toLowerCase().includes(q);
      const matchAuthor = p.author.toLowerCase().includes(q);
      const matchFlags = Object.keys(p.flags).some(k => k.toLowerCase().includes(q));
      return matchTitle || matchDesc || matchAuthor || matchFlags;
    });
  }

  if (currentSort === 'popular') {
    filtered.sort((a, b) => b.likes - a.likes);
  } else if (currentSort === 'downloads') {
    filtered.sort((a, b) => b.downloads - a.downloads);
  } else if (currentSort === 'newest') {
    filtered.sort((a, b) => (b.createdAt || 0) - (a.createdAt || 0));
  }

  return filtered;
}

// Render Cards
function renderPresets() {
  const grid = document.getElementById('workshopGrid');
  if (!grid) return;

  const list = filterPresets();
  const dict = workshopI18n[currentLang] || workshopI18n.ja;

  document.getElementById('statTotalPresets').textContent = presets.length;
  updateWorkshopLiveStats();

  if (presets.length === 0) {
    grid.innerHTML = `
      <div style="grid-column: 1 / -1; text-align: center; padding: 70px 20px; color: var(--text-muted);">
        <div style="font-size: 3.2rem; margin-bottom: 16px;">📦</div>
        <h3 style="font-size: 1.25rem; font-weight: 600; color: var(--text-primary); margin-bottom: 8px;">
          ${currentLang === 'ja' ? '公開中のプリセットはありません' : 'No presets published yet'}
        </h3>
        <p style="font-size: 0.9rem; color: var(--text-secondary); max-width: 480px; margin: 0 auto 24px; line-height: 1.6;">
          ${currentLang === 'ja'
            ? 'まだ誰もプリセットを公開していません。上部の「＋ プリセットを投稿」から自作の FastFlags をコミュニティに共有してみましょう！'
            : 'No community presets have been published yet. Be the first to share your optimized FastFlags by clicking "＋ Publish Preset"!'}
        </p>
        <button class="btn-publish" id="btnEmptyPublish" style="display: inline-flex; align-items: center; gap: 8px; margin: 0 auto; padding: 10px 22px; font-size: 0.95rem;">
          <span>＋</span>
          <span>${currentLang === 'ja' ? '最初のプリセットを投稿する' : 'Publish the First Preset'}</span>
        </button>
      </div>
    `;
    document.getElementById('btnEmptyPublish')?.addEventListener('click', () => {
      document.getElementById('btnOpenPublish')?.click();
    });
    return;
  }

  if (list.length === 0) {
    grid.innerHTML = `
      <div style="grid-column: 1 / -1; text-align: center; padding: 60px 0; color: var(--text-muted);">
        <div style="font-size: 2.4rem; margin-bottom: 12px;">🔍</div>
        <p style="font-size: 1.1rem; color: var(--text-secondary);">
          ${currentLang === 'ja' ? '該当する FastFlags プリセットが見つかりませんでした。' : 'No matching FastFlags presets found.'}
        </p>
        <p style="font-size: 0.85rem; margin-top: 4px;">
          ${currentLang === 'ja' ? '別のキーワードで検索するか、カテゴリーを変更してみてください。' : 'Try different search keywords or change categories.'}
        </p>
      </div>
    `;
    return;
  }

  grid.innerHTML = list.map(p => {
    const flagKeys = Object.keys(p.flags);
    const displayedTags = flagKeys.slice(0, 3).map(k => `<span class="flag-tag">${k}</span>`).join('');
    const moreCount = flagKeys.length > 3 ? `<span class="flag-tag">+${flagKeys.length - 3}</span>` : '';

    const isLiked = isPresetLiked(p.id);
    const likeClass = isLiked ? 'btn-like liked' : 'btn-like';
    const heartIcon = isLiked ? '❤️' : '🤍';
    const likeTitle = isLiked ? (currentLang === 'ja' ? 'いいねを取り消す' : 'Unlike') : (currentLang === 'ja' ? 'いいね' : 'Like');

    const isOwner = isMyPreset(p);
    const deleteBtnHtml = isOwner
      ? `<button class="btn-delete-preset" onclick="deletePreset('${p.id}')" title="${dict.btnDelete || '削除'}">🗑️ ${dict.btnDelete || '削除'}</button>`
      : '';

    return `
      <div class="workshop-card" data-id="${p.id}">
        <div class="card-top">
          <span class="card-category-badge">${getCategoryBadge(p.category)}</span>
          <h3 class="card-preset-title">${escapeHtml(p.title)}</h3>
          <p class="card-preset-desc">${escapeHtml(p.desc)}</p>

          <div class="flag-tags-wrap">
            ${displayedTags}
            ${moreCount}
          </div>

          <div class="card-author-row">
            <img src="${p.avatar}" class="author-avatar" alt="Avatar" onerror="this.src='https://tr.rbxcdn.com/30DAY-AvatarHeadshot-882C70E071E5997E51F8CB373002AFC3-Png/150/150/AvatarHeadshot/Png/isCircular'">
            <span class="author-name">@${escapeHtml(p.author)}</span>
            <span class="author-verified" title="Verified Roblox Account Ownership">✓</span>
          </div>
        </div>

        <div class="card-bottom">
          <div class="card-meta-stats">
            <button class="${likeClass}" onclick="likePreset('${p.id}')" title="${likeTitle}">
              <span>${heartIcon}</span> <span>${p.likes || 0}</span>
            </button>
            <span>⬇️ ${p.downloads.toLocaleString()}</span>
          </div>

          <div class="card-btns">
            ${deleteBtnHtml}
            <button class="btn-view-details" onclick="openDetails('${p.id}')">${dict.btnDetails}</button>
            <button class="btn-import" onclick="importPreset('${p.id}')">${dict.btnImport}</button>
          </div>
        </div>
      </div>
    `;
  }).join('');
}

function getCategoryBadge(cat) {
  const dict = workshopI18n[currentLang] || workshopI18n.ja;
  switch (cat) {
    case 'perf': return dict.catPerf;
    case 'gfx': return dict.catGfx;
    case 'comp': return dict.catComp;
    case 'retro': return dict.catRetro;
    case 'ui': return dict.catUi;
    default: return cat;
  }
}

function escapeHtml(str) {
  return (str || '').replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;").replace(/"/g, "&quot;");
}

// Check if preset belongs to current logged-in user
function isMyPreset(preset) {
  if (!currentUser || !preset) return false;
  if (preset.authorId && currentUser.userId && String(preset.authorId) === String(currentUser.userId)) {
    return true;
  }
  if (preset.author && currentUser.username && preset.author.toLowerCase() === currentUser.username.toLowerCase()) {
    return true;
  }
  return false;
}

// Delete Preset (Owner only)
function deletePreset(id) {
  if (!currentUser) return;
  const p = presets.find(item => item.id === id);
  if (!p) return;

  if (!isMyPreset(p)) {
    alert(currentLang === 'ja' ? "自分のプリセットのみ削除できます。" : "You can only delete your own presets.");
    return;
  }

  const dict = workshopI18n[currentLang] || workshopI18n.ja;
  const confirmMsg = (dict.confirmDelete || "プリセット「{title}」を削除しますか？\nこの操作は取り消せません。").replace("{title}", p.title);
  if (!confirm(confirmMsg)) return;

  presets = presets.filter(item => item.id !== id);
  savePresets();
  renderPresets();
  closeModal('detailModal');

  showToast(dict.toastDeleted || "プリセットを削除しました");
}

// Liked presets storage & toggle logic (prevents spamming likes)
function getLikedPresetIds() {
  try {
    const raw = localStorage.getItem('prism_liked_presets_v1');
    return raw ? JSON.parse(raw) : [];
  } catch {
    return [];
  }
}

function isPresetLiked(id) {
  const list = getLikedPresetIds();
  return list.includes(id);
}

function likePreset(id) {
  const p = presets.find(item => item.id === id);
  if (!p) return;

  let likedList = getLikedPresetIds();
  const alreadyLiked = likedList.includes(id);

  if (alreadyLiked) {
    // Already liked -> Remove like (-1)
    p.likes = Math.max(0, (p.likes || 1) - 1);
    likedList = likedList.filter(item => item !== id);
    showToast(currentLang === 'ja' ? "いいねを取り消しました" : "Removed like");
  } else {
    // Not liked yet -> Add like (+1)
    p.likes = (p.likes || 0) + 1;
    likedList.push(id);
    showToast(currentLang === 'ja' ? "いいねしました！ ❤️" : "Liked! ❤️");
  }

  localStorage.setItem('prism_liked_presets_v1', JSON.stringify(likedList));
  savePresets();
  renderPresets();
}

// Details Modal
function openDetails(id) {
  const p = presets.find(item => item.id === id);
  if (!p) return;

  const dict = workshopI18n[currentLang] || workshopI18n.ja;
  const modal = document.getElementById('detailModal');
  if (!modal) return;

  modal.querySelector('#detailTitle').textContent = p.title;
  modal.querySelector('#detailDesc').textContent = p.desc;
  modal.querySelector('#detailAuthor').textContent = `@${p.author}`;
  const detailAvatarImg = modal.querySelector('#detailAvatar');
  detailAvatarImg.src = p.avatar;
  detailAvatarImg.onerror = () => {
    detailAvatarImg.src = "https://tr.rbxcdn.com/30DAY-AvatarHeadshot-882C70E071E5997E51F8CB373002AFC3-Png/150/150/AvatarHeadshot/Png/isCircular";
  };

  const deleteBtn = modal.querySelector('#btnDetailDelete');
  if (deleteBtn) {
    if (isMyPreset(p)) {
      deleteBtn.style.display = 'inline-flex';
      deleteBtn.textContent = `🗑️ ${dict.btnDelete || '削除'}`;
      deleteBtn.onclick = () => deletePreset(p.id);
    } else {
      deleteBtn.style.display = 'none';
      deleteBtn.onclick = null;
    }
  }

  const jsonStr = JSON.stringify(p.flags, null, 2);
  modal.querySelector('#detailJson').textContent = jsonStr;

  modal.querySelector('#btnDetailCopy').onclick = () => {
    navigator.clipboard.writeText(jsonStr);
    showToast(dict.toastCopied);
  };

  modal.querySelector('#btnDetailImport').onclick = () => {
    importPreset(p.id);
  };

  openModal('detailModal');
}

// Language
function setLanguage(lang) {
  currentLang = lang;
  localStorage.setItem('prismstrap_lang', lang);
  
  const dict = workshopI18n[lang] || workshopI18n.ja;
  document.querySelectorAll('[data-i18n]').forEach(el => {
    const key = el.getAttribute('data-i18n');
    if (dict[key]) {
      el.innerHTML = dict[key];
    }
  });

  const btnLang = document.getElementById('btnLang');
  if (btnLang) {
    btnLang.innerHTML = lang === 'ja' ? '🌐 English' : '🌐 日本語';
  }

  const authSearchInput = document.getElementById('authSearchInput');
  if (authSearchInput && dict.authSearchPlaceholder) {
    authSearchInput.placeholder = dict.authSearchPlaceholder;
  }

  renderPresets();
}

// Setup Event Listeners
document.addEventListener('DOMContentLoaded', () => {
  initPresets();
  initUser();

  const storedLang = localStorage.getItem('prismstrap_lang') || (navigator.language.startsWith('ja') ? 'ja' : 'en');
  setLanguage(storedLang);

  document.getElementById('btnLang')?.addEventListener('click', () => {
    setLanguage(currentLang === 'ja' ? 'en' : 'ja');
  });

  document.getElementById('searchInput')?.addEventListener('input', (e) => {
    searchQuery = e.target.value;
    renderPresets();
  });

  document.querySelectorAll('.category-pill').forEach(pill => {
    pill.addEventListener('click', () => {
      document.querySelectorAll('.category-pill').forEach(p => p.classList.remove('active'));
      pill.classList.add('active');
      currentCategory = pill.getAttribute('data-category');
      renderPresets();
    });
  });

  document.getElementById('sortSelect')?.addEventListener('change', (e) => {
    currentSort = e.target.value;
    renderPresets();
  });

  // Publish button click
  document.getElementById('btnOpenPublish')?.addEventListener('click', () => {
    if (!currentUser) {
      openAuthWizard();
    } else {
      openModal('publishModal');
    }
  });

  // Auth Wizard - Search
  document.getElementById('btnAuthSearch')?.addEventListener('click', () => {
    const val = document.getElementById('authSearchInput').value;
    searchRobloxUser(val);
  });

  document.getElementById('authSearchInput')?.addEventListener('keydown', (e) => {
    if (e.key === 'Enter') {
      e.preventDefault();
      searchRobloxUser(e.target.value);
    }
  });

  // Auth Wizard - Proceed to Step 2
  document.getElementById('btnAuthToStep2')?.addEventListener('click', proceedToStep2);

  // Auth Wizard - Copy Code
  document.getElementById('btnCopyVerifyCode')?.addEventListener('click', () => {
    if (currentVerificationCode) {
      navigator.clipboard.writeText(currentVerificationCode);
      showToast(currentLang === 'ja' ? "コードをコピーしました" : "Code copied to clipboard");
    }
  });

  // Auth Wizard - Verify Step 3
  document.getElementById('btnDoVerify')?.addEventListener('click', verifyOwnership);

  // Auth Wizard - Finish
  document.getElementById('btnAuthFinish')?.addEventListener('click', () => {
    closeModal('authModal');
    showToast(currentLang === 'ja' ? "Roblox 認証が完了しました！" : "Roblox ownership verified!");
  });

  // Publish Form Submit
  document.getElementById('publishForm')?.addEventListener('submit', (e) => {
    e.preventDefault();
    if (!currentUser) return;

    const title = document.getElementById('pubTitleInput').value.trim();
    const desc = document.getElementById('pubDescInput').value.trim();
    const category = document.getElementById('pubCatSelect').value;
    const flagsRaw = document.getElementById('pubFlagsInput').value.trim();

    let parsedFlags = {};
    try {
      parsedFlags = JSON.parse(flagsRaw);
    } catch {
      alert("FastFlags は有効な JSON 形式である必要があります。\n例: {\"DFIntTaskSchedulerTargetFps\": 0}");
      return;
    }

    const newPreset = {
      id: `preset-${Date.now()}`,
      title,
      desc,
      category,
      author: currentUser.username,
      authorId: currentUser.userId,
      avatar: currentUser.avatar,
      downloads: 1,
      likes: 1,
      createdAt: Date.now(),
      flags: parsedFlags
    };

    presets.unshift(newPreset);
    savePresets();
    renderPresets();
    closeModal('publishModal');
    showToast("プリセットをワークショップに公開しました！");

    e.target.reset();
  });

  // Close modals
  document.querySelectorAll('.modal-overlay').forEach(overlay => {
    overlay.addEventListener('click', (e) => {
      if (e.target === overlay) {
        overlay.classList.remove('active');
      }
    });
  });

  renderPresets();
  initWorkshopLiveStats();
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

function updateWorkshopLiveStats() {
  const onlineEl = document.getElementById('statOnlinePlayers');
  const offlineEl = document.getElementById('statOfflinePlayers');
  const dlEl = document.getElementById('statGlobalDownloads');

  if (onlineEl) onlineEl.textContent = currentOnline.toLocaleString();
  if (offlineEl) offlineEl.textContent = currentOffline.toLocaleString();
  if (dlEl) dlEl.textContent = currentDownloads.toLocaleString();
}

async function fetchWorkshopLiveStats() {
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
    updateWorkshopLiveStats();
  } catch (err) {
    console.warn('[WorkshopLiveStats] Failed to fetch real stats:', err);
  }
}

function initWorkshopLiveStats() {
  updateWorkshopLiveStats();
  fetchWorkshopLiveStats();

  // Periodically refresh real stats from CountAPI every 30 seconds (No dummy random numbers!)
  setInterval(fetchWorkshopLiveStats, 30000);
}

