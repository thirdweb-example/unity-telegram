![Group 2 (1)](https://github.com/user-attachments/assets/1bb43b44-006d-4a1c-a41b-61eb718d3efd)

# thirdweb Telegram Mini App Example (Unity WebGL)

Easily embed your Unity WebGL blockchain game into Telegram Mini-Apps using thirdweb.

https://github.com/user-attachments/assets/7f542c59-2f9e-43bb-8dad-d45f1a065e62

## Getting Started

> This project assumes some basic knowledge of Unity, TypeScript, Next.js App Router, and [Connect SDK](https://portal.thirdweb.com/unity).

## Environment Variables

1. Create your `.env` by running `cp .env.example .env` in both the `/next-app` and `/telegram-bot` directories.

2. Create a client ID from the [thirdweb dashboard](https://thirdweb.com/dashboard/settings/api-keys) and add it to your `.env` as `NEXT_PUBLIC_THIRDWEB_CLIENT_ID`. Follow the instructions in each `.env` file to set up your environment variables.

## Set the authentication endpoint

This project uses a powerful thirdweb feature called [Authentication Endpoints](https://portal.thirdweb.com/connect/in-app-wallet/custom-auth/custom-auth-server). It uses your own API endpoint to generate a wallet for users on successful authentication. All the code for this is written for you in this project, you'll just need to set the endpoint in your thirdweb dashboard.

> To use Custom Authentication Endpoints, you'll need to be on the Growth Plan. If you have questions about the plan options or want to try it out, [reach out to our team](https://thirdweb.com/contact-us).

Navigate to the [In-App Wallets](https://thirdweb.com/dashboard/connect/in-app-wallets) page on the dashboard and select your project from the dropdown. **This should be the same project your `clientId` is from.** Then click the **"Configuration" tab** and scroll down to "Custom Authentication Endpoint" and enable the toggle. You'll then see a field to enter your endpoint.

<img width="1196" alt="Screenshot 2024-08-02 at 2 24 00 AM" src="https://github.com/user-attachments/assets/7cd1201f-1928-4fbc-8b8c-62c9cbe92833">

While testing the project locally, you'll need a publicly exposed endpoint to authenticate through. We recommend using a tool like [ngrok](https://ngrok.com/product/secure-tunnels) to create a public endpoint that forwards traffic to your local server. Forward your traffic to `http://localhost:3000` (where your app will run locally).

Once you have your ngrok or similar endpoint, add it to the Authentication Endpoint field as `[YOUR FORWARDING ENDPOINT]/api/auth/telegram`, the route this app uses to perform authentication.

You're now ready to run the project!

> **When you deploy to production (or any live URL), you'll modify this authentication endpoint to be your actual live URL. You could also create a separate thirdweb project for local development and production.**

### Run the project

You're now ready to test the project! First, you'll need to install the dependencies. Run the following command in both the `/next-app` and `/telegram-bot` directories:

```bash
pnpm install
```

Now, run `pnpm dev` in both the `/next-app` and `/telegram-bot` directories. This will start the Next.js app and the Telegram bot.

You should see the app at http://localhost:3000. Try messaging the `/start` command to the bot you configured with the Bot Father in Telegram.

Launching the app should have no effect at this point, we must create a Unity build to serve now.

## Building Unity for Telegram

Open the Unity example project, it already has thirdweb's [Unity SDK](https://github.com/thirdweb-dev/unity-sdk) imported.

1. Open `Scene_TelegramExample`

2. Set the client id you previously created in your `ThirdwebManager`

3. Build using your own WebGLTemplate or our provided MinimalFullScreen template. Unity SDK v5 has no strict template requirements.

4. Copy the Build folder's outputs to this repo's `/next-app/public/unity-webgl`. (It should have Build folder and index.html).

That's it, start the bot and you should see Unity load and after a few seconds, your wallet will be connected (default is Smart Wallet on Arbitrum Sepolia).

## How it works

The callback from the telegram bot starting passes the auth payload to unity through query params for simplicity.

From there, you simply use the Unity SDK's `InAppWallet` + `AuthEndpoint` login!

In this case, we take it one step further and upgrade it to a Smart Wallet, making gas sponsorship automatic and easy using ERC-4337 or zkSync Native Account Abstraction.

Note that you must run the Unity build from Telegram for it to work properly.

```csharp
var connection = new WalletOptions(
    provider: WalletProvider.InAppWallet,
    chainId: 421614,
    inAppWalletOptions: new InAppWalletOptions(
        authprovider: AuthProvider.AuthEndpoint,
        jwtOrPayload: JsonConvert.SerializeObject(payload),
    ),
    smartWalletOptions: new SmartWalletOptions(sponsorGas: true)
);
var smartWallet = await ThirdwebManager.Instance.ConnectWallet(connection);
var address = await smartWallet.GetAddress();
```

![image](https://github.com/user-attachments/assets/e07caf79-5908-4fc2-9c53-bce02fa3b9c7)

## Support

For help or feedback, please [visit our support site](https://thirdweb.com/support)
